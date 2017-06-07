using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.MySQL
{
    public class Connecter
    {
        MySqlConnection con;

        public Connecter(string server, string user, string password, string database)
        {
            try
            {
                con = new MySqlConnection($"server={server};uid={user};pwd={password};database={database};Allow User Variables=true");
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public MySqlConnection GetCon()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                return con;
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public int ExeNonQuery(MySqlCommand cmd)
        {
            using (cmd.Connection = GetCon())
            {
                int rowsAffected = -1;
                try
                {
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Debug.WriteLine(e);
                }
                return rowsAffected;
            }
        }

        public Task<int> ExeNonQueryAsync(MySqlCommand cmd)
        {
            using (cmd.Connection = GetCon())
            {
                Task<int> rowsAffected = cmd.ExecuteNonQueryAsync();
                return rowsAffected;
            }
        }

        public object ExeScalar(MySqlCommand cmd)
        {
            using (cmd.Connection = GetCon())
            {
                object obj = -1;
                obj = cmd.ExecuteScalar();
                return obj;
            }
        }

        public Task<object> ExeScalarAsync(MySqlCommand cmd)
        {
            using (cmd.Connection = GetCon())
            {
                Task<object> obj = cmd.ExecuteScalarAsync();
                return obj;
            }
        }

        public DataTable ExeReader(MySqlCommand cmd)
        {
            using (cmd.Connection = GetCon())
            {
                MySqlDataReader sdr;
                DataTable dt = new DataTable();
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                return dt;
            }
        }

        public Task<DbDataReader> ExeReaderAsync(MySqlCommand cmd)
        {
            using (cmd.Connection = GetCon())
            {
                Task<DbDataReader> sdr;
                sdr = cmd.ExecuteReaderAsync();
                return sdr;
            }
        }


    }
}
