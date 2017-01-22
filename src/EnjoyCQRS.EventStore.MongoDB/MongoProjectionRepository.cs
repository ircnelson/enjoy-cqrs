using EnjoyCQRS.EventSource.Projections;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnjoyCQRS.EventStore.MongoDB
{
    public class MongoProjectionRepository : IProjectionRepository, IDisposable
    {
        public MongoClient Client { get; }
        public string Database { get; }
        public MongoEventStoreSetttings Setttings { get; }

        public MongoProjectionRepository(MongoClient client, string database) : this(client, database, new MongoEventStoreSetttings())
        {
        }

        public MongoProjectionRepository(MongoClient client, string database, MongoEventStoreSetttings setttings, AddOrUpdateProjectionsDelegate addOrUpdateProjections = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (setttings == null) throw new ArgumentNullException(nameof(setttings));

            setttings.Validate();

            Database = database;
            Setttings = setttings;
            Client = client;
        }

        public async Task<Dictionary<string, object>> GetAsync(string name)
        {
            var builderFilter = Builders<MongoProjection>.Filter;
            var filter = builderFilter.Eq(x => x.Id, name);

            var projection = await QuerySingleResult(filter);

            return projection;
        }     

        public async Task<Dictionary<string, object>> GetAsync(string category, Guid id)
        {
            var builderFilter = Builders<MongoProjection>.Filter;
            var filter = builderFilter.Eq(x => x.Category, category) 
                & builderFilter.Eq(x => x.ProjectionId, id);

            var projection = await QuerySingleResult(filter);

            return projection;
        }

        private async Task<Dictionary<string, object>> QuerySingleResult(FilterDefinition<MongoProjection> filter)
        {
            var db = Client.GetDatabase(Database);
            var collection = db.GetCollection<MongoProjection>(Setttings.ProjectionsCollectionName);

            var projection = await collection
                .Find(filter)
                .Limit(1)
                .FirstAsync();

            var jsonSettings = new JsonWriterSettings {
                OutputMode = JsonOutputMode.Strict
            };

            var json = projection.Projection.ToJson(jsonSettings);

            var dictionary = BsonSerializer.Deserialize<Dictionary<string, object>>(json);

            // ugly, but necessary :(
            object id;

            if (dictionary.TryGetValue("_id", out id))
            {
                dictionary.Remove("_id");
                dictionary.Add("id", id);
            }
                        
            return dictionary;
        }

        public void Dispose()
        {
        }
    }
}
