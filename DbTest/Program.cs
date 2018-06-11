using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DbTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync();
            Console.ReadKey();
        }

        static async Task MainAsync()
        {
            var connectionString = "mongodb://admin:abc123!@localhost";
            var dataBase = "DataProviderDb";

            var client = new MongoClient( connectionString);
            var db = client.GetDatabase(dataBase);

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("BankAccounts");
            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        Console.WriteLine(document.GetValue("UserId"));
                        Console.WriteLine();
                    }
                }
            }

            var filter = new BsonDocument("UserId", "39488374-2993-3948-5938-394883720428");
            await collection.Find(filter)
                .ForEachAsync(document => Console.WriteLine(document));
        }
    }
}
