using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Graph
{
    public class Connecter
    {
        ISession conn;
        IDriver driver;

        public Connecter(string host, string user, string password)
        {
            driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "root"));
        }

        public ISession GetConnection()
        {
            if (conn == null)
            {
                conn = driver.Session();
            }
            try
            {
                return conn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
