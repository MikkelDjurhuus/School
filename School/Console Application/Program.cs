using Database;
using Database.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Document;

namespace Console_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            SocialNetwork sn = new SocialNetwork();
            //sn.SQL();
            sn.Neo4j();

            //Twitter tw = new Twitter();
            //tw.RunAll();

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
