using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Document
{
    public class Connecter
    {
        IMongoClient client = null;
        IMongoDatabase database = null;

        public Connecter(string db)
        {
            client = new MongoClient();
            database = client.GetDatabase(db);
        }

        IMongoDatabase GetConnection()
        {
            try
            {
                return database;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
