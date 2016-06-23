using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using EnjoyCQRS.Events;
using EnjoyCQRS.EventSource;
using EnjoyCQRS.EventSource.Snapshots;
using EnjoyCQRS.EventSource.Storage;
using Newtonsoft.Json;

namespace EnjoyCQRS.IntegrationTests.Sqlite
{
    public class EventStoreSqlite : IEventStore
    {
        private SQLiteConnection Connection { get; set; }
        private SQLiteTransaction Transaction { get; set; }

        public string FileName { get; }
        public bool SaveSnapshotCalled { get; private set; }
        public bool GetSnapshotCalled { get; private set; }

        public EventStoreSqlite(string fileName)
        {
            FileName = fileName;

            Connection = new SQLiteConnection($"Data Source={fileName}");
        }
        
        public void BeginTransaction()
        {
            if (Transaction != null) throw new InvalidOperationException("The transaction already opened.");

            EnsureOpenedConnection();

            Transaction = Connection.BeginTransaction();
        }

        public Task CommitAsync()
        {
            if (Transaction == null) throw new InvalidOperationException("The transaction is not open.");

            Transaction.Commit();
            
            Connection.Close();

            return Task.CompletedTask;
        }

        public void Rollback()
        {
            if (Transaction?.Connection != null)
                Transaction.Rollback();
        }

        public async Task<IEnumerable<ICommitedEvent>> GetAllEventsAsync(Guid id)
        {
            var command = Connection.CreateCommand();
            command.CommandText = "SELECT Body, Metadata FROM Events WHERE AggregateId = @AggregateId ORDER BY Version ASC";
            command.Parameters.AddWithValue("@AggregateId", id);

            var events = new List<ICommitedEvent>();

            EnsureOpenedConnection();

            using (command)
            using (var sqlReader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (sqlReader.Read())
                {
                    events.Add(Deserialize<ICommitedEvent>(sqlReader.GetString(0), sqlReader.GetString(1)));
                }
            }

            return events;
        }
        
        public async Task SaveAsync(IEnumerable<ISerializedEvent> collection)
        {
            var command = Connection.CreateCommand();
            command.CommandText = "INSERT INTO Events (Id, AggregateId, Timestamp, Metadata, Body, Version) VALUES (@Id, @AggregateId, @Timestamp, @Metadata, @Body, @Version)";
            command.Parameters.Add("@Id", DbType.Guid);
            command.Parameters.Add("@AggregateId", DbType.Guid);
            command.Parameters.Add("@Timestamp", DbType.DateTime);
            command.Parameters.Add("@Metadata", DbType.String);
            command.Parameters.Add("@Body", DbType.String);
            command.Parameters.Add("@Version", DbType.Int32);

            EnsureOpenedConnection();

            using (command)
            {
                foreach (var @event in collection)
                {
                    command.Parameters[0].Value = @event.Metadata.GetValue(MetadataKeys.EventId, Guid.Parse);
                    command.Parameters[1].Value = @event.Metadata.GetValue(MetadataKeys.AggregateId, Guid.Parse);
                    command.Parameters[2].Value = DateTime.UtcNow;
                    command.Parameters[3].Value = @event.SerializedMetadata;
                    command.Parameters[4].Value = @event.SerializedData;
                    command.Parameters[5].Value = @event.Metadata.GetValue(MetadataKeys.EventVersion, int.Parse);

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task SaveSnapshotAsync<TSnapshot>(TSnapshot snapshot) where TSnapshot : ISnapshot
        {
            var command = Connection.CreateCommand();
            command.CommandText = "INSERT INTO Snapshots (AggregateId, Timestamp, Body, Version) VALUES (@AggregateId, @Timestamp, @Body, @Version)";
            command.Parameters.Add("@AggregateId", DbType.Guid);
            command.Parameters.Add("@Timestamp", DbType.DateTime);
            command.Parameters.Add("@Body", DbType.String);
            command.Parameters.Add("@Version", DbType.Int32);

            EnsureOpenedConnection();

            command.Parameters[0].Value = snapshot.AggregateId;
            command.Parameters[1].Value = DateTime.UtcNow;
            command.Parameters[2].Value = Serialize(snapshot);
            command.Parameters[3].Value = snapshot.Version;

            await command.ExecuteNonQueryAsync().ConfigureAwait(false);

            SaveSnapshotCalled = true;
        }
        
        public async Task<ISnapshot> GetSnapshotByIdAsync(Guid aggregateId)
        {
            var command = Connection.CreateCommand();
            command.CommandText = "SELECT Body FROM Snapshots WHERE AggregateId = @AggregateId ORDER BY Version DESC LIMIT 1";
            command.Parameters.AddWithValue("@AggregateId", aggregateId);
            
            EnsureOpenedConnection();

            Snapshot snapshot = null;

            using (command)
            using (var sqlReader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (await sqlReader.ReadAsync())
                {
                    snapshot = Deserialize<Snapshot>(sqlReader.GetString(0));
                    break;
                }
            }

            GetSnapshotCalled = true;

            return snapshot;
        }
        
        public async Task<IEnumerable<ICommitedEvent>> GetEventsForwardAsync(Guid aggregateId, int version)
        {
            var command = Connection.CreateCommand();
            command.CommandText = "SELECT Body, Metadata FROM Events WHERE AggregateId = @AggregateId AND Version > @Version ORDER BY Version ASC";
            command.Parameters.AddWithValue("@AggregateId", aggregateId);
            command.Parameters.AddWithValue("@Version", version);

            List<ICommitedEvent> events = new List<ICommitedEvent>();

            EnsureOpenedConnection();

            using (command)
            using (var sqlReader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (await sqlReader.ReadAsync())
                {
                    events.Add(Deserialize<ICommitedEvent>(sqlReader.GetString(0), sqlReader.GetString(1)));
                }
            }

            return events;
        }

        public void Dispose()
        {
            Rollback();

            Connection?.Dispose();
            Transaction?.Dispose();

            Connection = null;
            Transaction = null;
        }

        private static string Serialize<TObject>(TObject @event)
        {
            return JsonConvert.SerializeObject(@event);
        }

        private static TReturn Deserialize<TReturn>(string json)
        {
            var @event = JsonConvert.DeserializeObject(json);

            return (TReturn)@event;
        }

        private static TReturn Deserialize<TReturn>(string json, string typeName)
        {
            var type = Type.GetType(typeName);

            var @event = JsonConvert.DeserializeObject(json, type);

            return (TReturn)@event;
        }
        
        private void EnsureOpenedConnection()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }
    }
}