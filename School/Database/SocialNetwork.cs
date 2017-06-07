using Database.MySQL;
using MySql.Data.MySqlClient;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class SocialNetwork
    {
        /// <summary>
        /// Neo4j
        /// Total time = 632, Median = 11, Avg = 31
        /// Total time = 472, Median = 21, Avg = 23
        /// Total time = 1063, Median = 35, Avg = 53
        /// Total time = 6959, Median = 263, Avg = 347
        /// Total time = 151458, Median = 5625, Avg = 7572
        /// SQL
        /// Total time = 143, Median = 6, Avg = 7
        /// Total time = 471, Median = 18, Avg = 23
        /// Total time = 13946, Median = 576, Avg = 697
        /// Total time = 315934, Median = 14023, Avg = 15796
        /// Total time = 8845764, Median = 352423, Avg = 442452
        /// </summary>


        int[] ids = { 402743, 118439, 439576, 485573, 300841, 129113, 249992, 348985, 94862, 240084, 126003, 118123, 418806, 314723, 396517, 40675, 481175, 45519, 260522, 119144 };


        public void SQL()
        {
            string[] sql = {
                "SELECT COUNT(DISTINCT e1.target) as endorses FROM endorsement as e1 WHERE e1.source = @id",
                "SELECT COUNT(DISTINCT e2.target) as endorses FROM endorsement as e1 INNER JOIN endorsement e2 ON e1.target = e2.source WHERE e1.source = @id",
                "SELECT COUNT(DISTINCT e3.target) as endorses FROM endorsement as e1 INNER JOIN endorsement e2 ON e1.target = e2.source JOIN endorsement e3 ON e2.target = e3.source WHERE e1.source = @id",
                "SELECT COUNT(DISTINCT e4.target) as endorses FROM endorsement as e1 INNER JOIN endorsement e2 ON e1.target = e2.source JOIN endorsement e3 ON e2.target = e3.source INNER JOIN endorsement e4 ON e3.target = e4.source WHERE e1.source = @id",
                "SELECT COUNT(DISTINCT e5.target) as endorses FROM endorsement as e1 INNER JOIN endorsement e2 ON e1.target = e2.source JOIN endorsement e3 ON e2.target = e3.source INNER JOIN endorsement e4 ON e3.target = e4.source INNER JOIN endorsement e5 ON e4.target = e5.source WHERE e1.source = @id"
            };
            for (int s = 0; s < sql.Length; s++)
            {
                using (MySqlConnection con = new MySqlConnection("server=127.0.0.1;uid=root;pwd=root;database=socialnetwork"))
                {
                    MySqlCommand cmd = new MySqlCommand(sql[s], con);
                    cmd.Parameters.AddWithValue("@id", ids[0]);
                    cmd.Prepare();
                    List<long> time = new List<long>();
                    for (int i = 0; i < ids.Length; i++)
                    {
                        cmd.Parameters[0].Value = ids[i];
                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        cmd.ExecuteNonQuery();
                        time.Add(watch.ElapsedMilliseconds);
                    }
                    GetStats(time);
                }
            }
        }

        public void Neo4j()
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "root")))
            {
                using (var session = driver.Session())
                {
                    for (int depth = 1; depth <= 5; depth++)
                    {
                        List<long> time = new List<long>();
                        for (int i = 0; i < ids.Length; i++)
                        {
                            Stopwatch watch = new Stopwatch();
                            watch.Start();
                            int id = ids[i];
                            var result = session.Run($"MATCH (:Person {{id: {id}}})-[:ENDORSES *{depth}]->(m) RETURN DISTINCT count(m) AS count");
                            Console.WriteLine(result.First()["count"].ToString());
                            time.Add(watch.ElapsedMilliseconds);
                        }
                        GetStats(time);
                    }
                }
            }
        }

        public void GetStats(List<long> list)
        {
            list.Sort();
            long sum = list.Sum();
            long median = list[list.Count / 2];
            long avg = sum / list.Count;
            Console.WriteLine($"Total time = {sum}, Median = {median}, Avg = {avg}");
        }
    }
}
