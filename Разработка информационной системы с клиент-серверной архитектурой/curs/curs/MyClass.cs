using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace curs
{
    public static class ВыводДомов
    {
        public static List<string> Адрес = new List<string>();
        public static List<string> КоличествоКвартир = new List<string>();
        public static List<string> КоличествоПодъездов = new List<string>();
        public static int ДлинаАдреса = new int();
        

        public static SqlDataReader выводДомов()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select * from Дом";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Адрес.Add(reader[0].ToString().Trim());
                КоличествоКвартир.Add(reader[1].ToString().Trim());
                КоличествоПодъездов.Add(reader[2].ToString().Trim());
            }
            con.Close();
            return reader;
        }
    }
}
