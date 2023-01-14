using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication2.Models
{
    public class UsersService
    {
        private readonly IMongoCollection<BsonDocument> table;

        public UsersService(IOptions<UsersDatabaseSetting> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            /*Console.WriteLine(options.Value.DatabaseName);
            Console.WriteLine(options.Value.UsersCollectionName);*/
            table = database.GetCollection<BsonDocument>(options.Value.UsersCollectionName);
        }

        public async Task Create(BsonDocument useridentity)
        {
            await table.InsertOneAsync(useridentity);
        }
        public async Task<List<BsonDocument>> Get()
        {
            return await table.Find(new BsonDocument()).ToListAsync();
        }

    }
}
