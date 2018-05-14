/***********************
 * BitAdmin2.0框架文件
 ***********************/
using {{cookiecutter.project_name}};
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace {{cookiecutter.project_name}}.Helpers
{
    public sealed class MongoDBHelper
    {
        public static string ConnectionString { get { return HttpContextCore.Configuration["ConnectionStrings:MongoDB"]; } }

        public static IMongoDatabase MongoDB;
        static MongoDBHelper()
        {
            MongoDB = new MongoClient(ConnectionString).GetDatabase("{{cookiecutter.project_name}}");
        }  
        public static void InsertOne(string key, BsonDocument doc)
        {
            var collection = GetCollection(key);
            collection.InsertOne(doc);
        }
        public static void InsertMany(string key, List<BsonDocument> docs)
        {
            var collection = GetCollection(key);
            collection.InsertMany(docs);
        }
        public static long Count(string key, BsonDocument doc)
        {
            if (doc == null) doc = new BsonDocument();
             var collection = GetCollection(key);
            return collection.Count(doc);
        }

        public static IMongoCollection<BsonDocument> GetCollection(string key)
        {
            return MongoDB.GetCollection<BsonDocument>(key);
        }
    }
}
