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
    public static class Issues
    {
        public static List<string> НомерЗаявки = new List<string>();
        public static List<string> ТипЗаявки = new List<string>();
        public static List<string> ДатаСоздания = new List<string>();
        public static List<string> СтатусЗаявки = new List<string>();
        public static List<string> Регистратор = new List<string>();
        public static List<string> Заявитель = new List<string>();
        public static List<string> Исполнитель = new List<string>();
        public static List<string> ДатаЗакрытия = new List<string>();
        public static List<string> Адрес = new List<string>();
        public static List<string> НомерКвартиры = new List<string>();
        public static List<string> Примечание = new List<string>();

        public static DataSet операторы = new DataSet();
        public static DataSet типыЗаявок = new DataSet();
        public static DataSet жильцы = new DataSet();

        public static DataSet статусыЗаявок = new DataSet();
        public static DataSet исполнители = new DataSet();

    }
}
