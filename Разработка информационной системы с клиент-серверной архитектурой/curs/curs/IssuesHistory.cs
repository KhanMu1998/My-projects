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
    public static class IssuesHistory
    {
        public static List<string> ID = new List<string>();
        public static List<string> НомерЗаявки = new List<string>();
        public static List<string> СтатусЗаявки = new List<string>();
        public static List<string> Исполнитель = new List<string>();
        public static List<string> Дата = new List<string>();
        public static List<string> Примечание = new List<string>();

    }
}
