using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.MySQL
{
    public class University
    {
        Connecter mysql;

        public University()
        {
            mysql = new Connecter("127.0.0.1", "root", "root", "university");
        }

        public int InsertCourse(string course_id, string title, string dep_name, int credits)
        {
            MySqlCommand cmd = new MySqlCommand("insert into course (course_id,title,dept_name,credits) values(@course,@title,@departement,@credits)");
            cmd.Parameters.AddWithValue("@course",course_id);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@department", dep_name);
            cmd.Parameters.AddWithValue("@credits", credits);
            return mysql.ExeNonQuery(cmd);
        }
    }
}
