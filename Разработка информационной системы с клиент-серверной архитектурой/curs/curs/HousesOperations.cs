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
    public static class HousesOperations
    {
        public static void выводДомов()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select * from Дом";
            SqlDataReader reader = cmd.ExecuteReader();
            Houses.Адрес.Clear();
            Houses.КоличествоКвартир.Clear();
            Houses.КоличествоПодъездов.Clear();
            while (reader.Read())
            {
                Houses.Адрес.Add(reader["Адрес"].ToString().Trim());
                Houses.КоличествоКвартир.Add(reader["КоличествоКвартир"].ToString().Trim());
                Houses.КоличествоПодъездов.Add(reader["КоличествоПодъездов"].ToString().Trim());
            }
            con.Close();
        }

        public static int добавлениеДома(string ПараметрАдресДома, string ПараметрКоличествоКвартир, string ПараметрКоличествоПодъездов)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertHome";

            cmd.Parameters.Add("@Адрес", SqlDbType.NVarChar, 250).Value = ПараметрАдресДома;
            cmd.Parameters.Add("@КоличествоКвартир", SqlDbType.Int).Value = ПараметрКоличествоКвартир;
            cmd.Parameters.Add("@КоличествоПодъездов", SqlDbType.Int).Value = ПараметрКоличествоПодъездов;

            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }
    }
}
