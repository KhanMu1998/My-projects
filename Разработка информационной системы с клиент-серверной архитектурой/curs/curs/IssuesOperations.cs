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
    public static class IssuesOperations
    {
        public static void вывестиЗаявки(bool всеЗаявки, bool толькоОткрытые, bool толькоЗакрытые, bool найтиЗаявку = false, int номерЗаявки = 0)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();
            SqlCommand cmd = con.CreateCommand();

            if (всеЗаявки)
            {
                cmd.CommandText = "select * from Заявки_Квартиры";
            }
            else if (толькоОткрытые)
            {
                cmd.CommandText = "select * from Заявки_Квартиры where СтатусЗаявки != @ЗакрытыеЗаявки";
                cmd.Parameters.Add("@ЗакрытыеЗаявки", SqlDbType.NVarChar, 250).Value = "Закрыта";
            }
            else if (толькоЗакрытые)
            {
                cmd.CommandText = "select * from Заявки_Квартиры where СтатусЗаявки = @ЗакрытыеЗаявки";
                cmd.Parameters.Add("@ЗакрытыеЗаявки", SqlDbType.NVarChar, 250).Value = "Закрыта";
            }
            if (найтиЗаявку)
            {
                cmd.CommandText = "select * from Заявки_Квартиры where НомерЗаявки = @НомерЗаявки";
                cmd.Parameters.Add("@НомерЗаявки", SqlDbType.Int).Value = номерЗаявки;
            }

            SqlDataReader reader = cmd.ExecuteReader();

            Issues.НомерЗаявки.Clear();
            Issues.ТипЗаявки.Clear();
            Issues.ДатаСоздания.Clear();
            Issues.СтатусЗаявки.Clear();
            Issues.Регистратор.Clear();
            Issues.Заявитель.Clear();
            Issues.Исполнитель.Clear();
            Issues.ДатаЗакрытия.Clear();
            Issues.Адрес.Clear();
            Issues.НомерКвартиры.Clear();
            Issues.Примечание.Clear();

            while (reader.Read())
            {
                Issues.НомерЗаявки.Add(reader["НомерЗаявки"].ToString().Trim());
                Issues.ТипЗаявки.Add(reader["ТипЗаявки"].ToString().Trim());
                Issues.ДатаСоздания.Add(reader["ДатаСоздания"].ToString().Substring(0, 16).Trim());
                Issues.СтатусЗаявки.Add(reader["СтатусЗаявки"].ToString().Trim());
                Issues.Регистратор.Add(reader["Регистратор"].ToString().Trim());
                Issues.Заявитель.Add(reader["Заявитель"].ToString().Trim());
                Issues.Исполнитель.Add(reader["Исполнитель"].ToString().Trim());
                Issues.ДатаЗакрытия.Add(reader["ДатаЗакрытия"].ToString().Trim());
                Issues.Адрес.Add(reader["Адрес"].ToString().Trim());
                Issues.НомерКвартиры.Add(reader["НомерКвартиры"].ToString().Trim());
                Issues.Примечание.Add(reader["Примечание"].ToString().Trim());
            }
            con.Close();
        }

        public static void добавитьЗаявку(string ТипЗаявки, string Регистратор, string Заявитель, string Примечание)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "insertIssue";

            cmd.Parameters.Add("@ТипЗаявки", SqlDbType.NVarChar, 150).Value = ТипЗаявки;
            cmd.Parameters.Add("@Регистратор", SqlDbType.NVarChar, 250).Value = Регистратор;
            if (Заявитель != "")
            {
                cmd.Parameters.Add("@Заявитель", SqlDbType.NVarChar, 250).Value = Заявитель;
            }
            cmd.Parameters.Add("@Примечание", SqlDbType.NVarChar, 250).Value = Примечание;

            int result = cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void удалитьЗаявку(string НомерЗаявки)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "deleteIssue";

            cmd.Parameters.Add("@НомерЗаявки", SqlDbType.Int).Value = НомерЗаявки;
            int result = cmd.ExecuteNonQuery();

            con.Close();
        }

        public static void обновитьЗаявку(string НомерЗаявки, string СтатусЗаявки, string Исполнитель, string Примечание)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "updateIssue";
            cmd.Parameters.Add("@НомерЗаявки", SqlDbType.Int).Value = НомерЗаявки;
            cmd.Parameters.Add("@СтатусЗаявки", SqlDbType.NVarChar, 50).Value = СтатусЗаявки;
            if (Исполнитель != "")
            {
                cmd.Parameters.Add("@Исполнитель", SqlDbType.NVarChar, 250).Value = Исполнитель;
            }
            cmd.Parameters.Add("@Примечание", SqlDbType.NVarChar, 250).Value = Примечание;

            int result = cmd.ExecuteNonQuery();


            con.Close();
        }

        public static void заполнитьВыпадающееМеню(string ТипЗаявки = "")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;

            con.Open();
            SqlCommand cmd = new SqlCommand("select ФИО from Сотрудник where Должность = 'Оператор'", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            Issues.операторы.Clear();
            da.Fill(Issues.операторы);

            cmd = new SqlCommand("select ТипЗаявки from ТипыЗаявок", con);
            da.SelectCommand = cmd;
            Issues.типыЗаявок.Clear();
            da.Fill(Issues.типыЗаявок);

            cmd = new SqlCommand("select ФИО from Квартира_ФизЛицо", con);
            da.SelectCommand = cmd;
            Issues.жильцы.Clear();
            da.Fill(Issues.жильцы);

            cmd = new SqlCommand("select СтатусЗаявки from СтатусыЗаявок", con);
            da.SelectCommand = cmd;
            Issues.статусыЗаявок.Clear();
            da.Fill(Issues.статусыЗаявок);

            if (ТипЗаявки != "") { 
                cmd = new SqlCommand("select ФИО from ТипыЗаявок_Должность_Сотрудники where ТипЗаявки = @ТипЗаявки", con);
                cmd.Parameters.Add("@ТипЗаявки", SqlDbType.NVarChar, 150).Value = ТипЗаявки;
                da.SelectCommand = cmd;
                Issues.исполнители.Clear();
                da.Fill(Issues.исполнители);
            }
            con.Close();
        }
    }
}
