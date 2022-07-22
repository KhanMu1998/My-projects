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
    class IssuesHistoryOperations
    {
        public static void вывестиИсториюЗаявки(bool ВСпискеЗаявокВыбранЭлемент, bool ВСпискеЗаявокВыбраноНеОглавление, string НомерЗаявки)
        {
            if ((ВСпискеЗаявокВыбранЭлемент) & (ВСпискеЗаявокВыбраноНеОглавление))
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from ИсторияЗаявки where НомерЗаявки = @НомерЗаявки";
                cmd.Parameters.Add("@НомерЗаявки", SqlDbType.Int).Value = НомерЗаявки;
                SqlDataReader reader = cmd.ExecuteReader();

                IssuesHistory.ID.Clear();
                IssuesHistory.НомерЗаявки.Clear();
                IssuesHistory.СтатусЗаявки.Clear();
                IssuesHistory.Исполнитель.Clear();
                IssuesHistory.Дата.Clear();
                IssuesHistory.Примечание.Clear();

                while (reader.Read())
                {
                    IssuesHistory.ID.Add(reader["ID"].ToString().Trim());
                    IssuesHistory.НомерЗаявки.Add(reader["НомерЗаявки"].ToString().Trim());
                    IssuesHistory.СтатусЗаявки.Add(reader["СтатусЗаявки"].ToString().Trim());
                    IssuesHistory.Исполнитель.Add(reader["Исполнитель"].ToString().Trim());
                    IssuesHistory.Дата.Add(reader["Дата"].ToString().Trim());
                    IssuesHistory.Примечание.Add(reader["Примечание"].ToString().Trim());
                }

                
                con.Close();
            }
        }
    }
}
