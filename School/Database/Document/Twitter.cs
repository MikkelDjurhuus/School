using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Database.Document
{
    public class Twitter
    {
        private IMongoDatabase database;
        public IMongoCollection<BsonDocument> tweets { get { return database.GetCollection<BsonDocument>("Tweet"); } }

        public Twitter()
        {
            MongoClient client = new MongoClient();
            database = client.GetDatabase("Twitter");
        }

        public void RunAll()
        {
            Console.WriteLine("0 = How many Twitter users are in our database?");
            Console.WriteLine("1 = Which Twitter users link the most to other Twitter users?");
            Console.WriteLine("2 = Who is are the most mentioned Twitter users?");
            Console.WriteLine("3 = Who are the most active Twitter users?");
            Console.WriteLine("4 = Who are the five most grumpy?");
            Console.WriteLine("5 = Who are the five most positive?");
            while (true)
            {
                Console.WriteLine("Chose function to run:");
                int i = 10;
                bool result = int.TryParse(Console.ReadLine().ToString(), out i);
                Console.WriteLine("Wait for result..");
                switch (i)
                {
                    case 0:
                        Query1();
                        break;
                    case 1:
                        Query2();
                        break;
                    case 2:
                        Query3();
                        break;
                    case 3:
                        Query4();
                        break;
                    case 4:
                        Query5Negative();
                        break;
                    case 5:
                        Query5Positive();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// How many Twitter users are in our database?
        /// </summary>
        public void Query1()
        {
            try
            {
                var filter = tweets.Distinct<dynamic>("user", new BsonDocument());
                var results = filter.ToList();
                Console.WriteLine(results.Count.ToString());

            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// Which Twitter users link the most to other Twitter users? (Provide the top ten.)
        /// </summary>
        public void Query2()
        {
            try
            {
                var aggregate = tweets.Aggregate()
                    .Match(new BsonDocument { { "text", new Regex(@"@\w+") } })
                    .Group(new BsonDocument { { "_id", "$user" }, { "count", new BsonDocument("$sum", 1) } })
                    .Sort(new BsonDocument("count", -1))
                    .Limit(10);
                var results = aggregate.ToList();
                foreach (var item in results)
                {
                    Console.WriteLine(item.ToString());
                }

            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// Who is are the most mentioned Twitter users? (Provide the top five.)
        /// </summary>
        public void Query3()
        {
            try
            {
                var options = new MapReduceOptions<BsonDocument, BsonDocument>();
                options.Filter = new BsonDocument { };
                options.Limit = 5;
                options.Sort = new BsonDocument { { "value", 1 } };
                var map = new BsonJavaScript(@"function(){var match=this.text.match(/" + new Regex(@"@\w+") + "/); var key=match?match[0]:null; if(key){emit(key,1);}}");
                var reduce = new BsonJavaScript(@"function(key,value){return Array.sum(value);}");
                var mapReduce = tweets.MapReduce(map, reduce, options).ToList();
                foreach (var item in mapReduce)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// Who are the most active Twitter users (top ten)?
        /// </summary>
        public void Query4()
        {
            try
            {
                var aggregate = tweets.Aggregate()
                    .Group(new BsonDocument { { "_id", "$user" }, { "count", new BsonDocument("$sum", 1) } })
                    .Sort(new BsonDocument("count", -1))
                    .Limit(10);
                var results = aggregate.ToList();
                foreach (var item in results)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// Who are the five most grumpy (most negative tweets) (Provide five users for each group)
        /// </summary>
        public void Query5Negative()
        {
            try
            {
                var aggregate = tweets.Aggregate()
                    .Match(new BsonDocument { { "polarity", 0 } })
                    .Group(new BsonDocument { { "_id", "$user" }, { "count", new BsonDocument("$sum", 1) } })
                    .Sort(new BsonDocument("count", -1))
                    .Limit(10);
                var results = aggregate.ToList();
                foreach (var item in results)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// Who are the most happy (most positive tweets)? (Provide five users for each group)
        /// </summary>
        public void Query5Positive()
        {
            try
            {
                var aggregate = tweets.Aggregate()
                    .Match(new BsonDocument { { "polarity", 4 } })
                    .Group(new BsonDocument { { "_id", "$user" }, { "count", new BsonDocument("$sum", 1) } })
                    .Sort(new BsonDocument("count", -1))
                    .Limit(10);
                var results = aggregate.ToList();
                foreach (var item in results)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
