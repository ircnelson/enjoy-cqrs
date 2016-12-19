#tool "nuget:?package=xunit.runner.console"
#tool "nuget:https://www.nuget.org/api/v2?package=OpenCover&version=4.6.519"
#tool "nuget:https://www.nuget.org/api/v2?package=ReportGenerator&version=2.4.5"
#tool "nuget:https://www.nuget.org/api/v2?package=coveralls.io&version=1.3.4"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var skipOpenCover = Argument("skipOpenCover", true);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var artifactsDir = (DirectoryPath) Directory("./artifacts");
var testResultsDir = (DirectoryPath) artifactsDir.Combine("test-results");
var testCoverageOutputFilePath = testResultsDir.CombineWithFilePath("OpenCover.xml");
var outputNugets = artifactsDir.Combine("nugets");

var isAppVeyorBuild = AppVeyor.IsRunningOnAppVeyor;
var buildVersion = "build" + Context.EnvironmentVariable("Build");
var branch = Context.EnvironmentVariable("APPVEYOR_REPO_BRANCH");
var coverallsToken = Context.EnvironmentVariable("COVERALLS_REPO_TOKEN");

Context.Information("Test Coverage Output File: " + testCoverageOutputFilePath);
Context.Information("Build Version: " + buildVersion);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./", new DotNetCoreRestoreSettings
    {
        Verbose = false,
        Verbosity = DotNetCoreRestoreVerbosity.Warning,
        Sources = new [] {
            "https://www.myget.org/F/xunit/api/v3/index.json",
            "https://dotnet.myget.org/F/dotnet-core/api/v3/index.json",
            "https://dotnet.myget.org/F/cli-deps/api/v3/index.json",
            "https://api.nuget.org/v3/index.json",
        }
    });
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
    }

    var projects = GetFiles("./**/*.xproj");
    
    foreach(var project in projects)
    {
        Context.Information("Project: " + project.GetDirectory().FullPath);

        DotNetCoreBuild(project.GetDirectory().FullPath, new DotNetCoreBuildSettings {
            VersionSuffix = Context.EnvironmentVariable("Build"),
            Configuration = configuration
        });
    }
    
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles("./test/**/*Tests.xproj");

    CreateDirectory(testResultsDir);

    Context.Information("Found " + projects.Count() + " projects");

    foreach(var project in projects)
    {
        if(IsRunningOnWindows())
        {
            var apiUrl = EnvironmentVariable("APPVEYOR_API_URL");

            try
            {
                if (!string.IsNullOrEmpty(apiUrl))
                {
                    // Disable XUnit AppVeyorReporter see https://github.com/cake-build/cake/issues/1200
                    System.Environment.SetEnvironmentVariable("APPVEYOR_API_URL", null);
                }

                Action<ICakeContext> testAction = tool => {

                    tool.DotNetCoreTest(project.GetDirectory().FullPath, new DotNetCoreTestSettings {
                        Configuration = configuration,
                        NoBuild = true,
                        Verbose = false,
                        ArgumentCustomization = args =>
                            args.Append("-xml").Append(testResultsDir.CombineWithFilePath(project.GetFilenameWithoutExtension()).FullPath + ".xml")
                    });
                };

                if (!skipOpenCover) {
                    OpenCover(testAction,
                        testCoverageOutputFilePath,
                        new OpenCoverSettings {
                            ReturnTargetCodeOffset = 0,
                            ArgumentCustomization = args => args.Append("-mergeoutput")
                        }
                        .WithFilter("+[EnjoyCQRS*]* -[xunit.*]* -[FluentAssertions*]* -[*Tests]* -[*Tests.Shared]* ")
                        .ExcludeByAttribute("*.ExcludeFromCodeCoverage*")
                        .ExcludeByFile("*/*Designer.cs;*/*.g.cs;*/*.g.i.cs"));
                } 
                else 
                {
                    testAction(Context);
                }
            }
            finally
            {
                if (!string.IsNullOrEmpty(apiUrl))
                {
                    System.Environment.SetEnvironmentVariable("APPVEYOR_API_URL", apiUrl);
                }
            }
        }
        else
        {
            var name = project.GetFilenameWithoutExtension();
            var dirPath = project.GetDirectory().FullPath;
            var xunit = GetFiles(dirPath + "/bin/" + configuration + "/net461/*/dotnet-test-xunit.exe").First().FullPath;
            var testfile = GetFiles(dirPath + "/bin/" + configuration + "/net461/*/" + name + ".dll").First().FullPath;

            using(var process = StartAndReturnProcess("mono", new ProcessSettings { Arguments = xunit + " " + testfile }))
            {
                process.WaitForExit();
                if (process.GetExitCode() != 0)
                {
                    throw new Exception("Mono tests failed!");
                }
            }
        }
    }

    // Generate the HTML version of the Code Coverage report if the XML file exists
    if (FileExists(testCoverageOutputFilePath))
    {
        ReportGenerator(testCoverageOutputFilePath, testResultsDir);
    }
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Build")
    .Does(() => 
{
    var nuspecs = GetFiles("./src/**/*.nuspec");

    foreach (var nuspec in nuspecs)
    {
        var dotNetCorePackSettings = new DotNetCorePackSettings {
            Configuration = configuration,
            OutputDirectory = outputNugets,
            NoBuild = true,
            Verbose = false
        };

        if (isAppVeyorBuild && branch != "master") 
        {
            dotNetCorePackSettings.VersionSuffix = buildVersion.ToString(),
        }

        DotNetCorePack(nuspec.GetDirectory().FullPath, dotNetCorePackSettings);
    }
});

Task("Code-Coverage")
    .WithCriteria(() => FileExists(testCoverageOutputFilePath))
    .WithCriteria(() => !Context.IsLocalBuild)
    .WithCriteria(() => string.IsNullOrEmpty(coverallsToken))
    .WithCriteria(() => branch == "master")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() => 
{
    CoverallsIo(testCoverageOutputFilePath, new CoverallsIoSettings()
    {
        RepoToken = coverallsToken
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Create-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests");

Task("AppVeyor")
    .IsDependentOn("Code-Coverage")
    .IsDependentOn("Create-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests");
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);