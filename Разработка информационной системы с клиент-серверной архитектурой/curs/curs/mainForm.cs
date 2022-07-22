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
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            ПанельВкладок.Appearance = TabAppearance.FlatButtons;
            ПанельВкладок.ItemSize = new Size(0, 1);
            ПанельВкладок.SizeMode = TabSizeMode.Fixed;
        }

        private void домToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ПанельВкладок.SelectTab(1);
            ВывестиДома_Click(sender, e);
        }

        private void наГлавнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ПанельВкладок.SelectTab(0);
        }

        private void физЛицоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ПанельВкладок.SelectTab(5);

            ЗаполнитьComboBoxВФизЛицах();
            ОчиститьФизЛиц_Click(sender, e);
            ВывестиФизЛиц_Click(sender, e);
        }

        private void квартираToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ПанельВкладок.SelectTab(4);
            ЗаполнитьComboBoxВКвартирах();
            АдресКвартиры.Text = "";
            ВывестиКвартиры_Click(sender, e);
        }

        private void КСозданиюЗаявки_Click(object sender, EventArgs e)
        {
            ПанельВкладок.SelectTab(2);
            
            
            ЗаполнитьВыпадающиеМеню();
            ОчиститьЗаявки_Click(sender, e);
            ВывестиЗаявки_Click(sender, e);

        }
        
        private void КОбновлениюЗаявки_Click(object sender, EventArgs e)
        {
            ПанельВкладок.SelectTab(3);
            
            
            ЗаполнитьВыпадающиеМеню();
            ОсчиститьЗаявкиСобновления_Click(sender, e);
            ВывестиЗаявки_Click(sender, e);
        }

        

        private void ВывестиДома_Click(object sender, EventArgs e)
        {
            
            HousesOperations.выводДомов();
            string listFormat = "{0,-25}{1,-25}{2,-25}";
            СписокДомов.Items.Clear();
            СписокДомов.Items.Add(String.Format(listFormat, "Адрес", "Количество Квартир", "Количество Подъездов"));

            for (int i = 0; i < Houses.Адрес.Count; i++)
            {
                СписокДомов.Items.Add(String.Format(listFormat,
                                      Houses.Адрес[i],
                                      Houses.КоличествоКвартир[i],
                                      Houses.КоличествоПодъездов[i]));
            }
               
        }


        private void ОчиститьСписокДомов_Click(object sender, EventArgs e)
        {
            СписокДомов.Items.Clear();
            АдресДома.Text = "";
            КоличествоКвартир.Text = "";
            КоличествоПодъездов.Text = "";
            ИзмененоДомов.Text = "";
        }

        private void ДобавитьДом_Click(object sender, EventArgs e)
        {
            string ПараметрАдресДома = АдресДома.Text.ToString();
            string ПараметрКоличествоКвартир = КоличествоКвартир.Text;
            string ПараметрКоличествоПодъездов = КоличествоПодъездов.Text;

            
            if (HousesOperations.добавлениеДома(ПараметрАдресДома, ПараметрКоличествоКвартир, ПараметрКоличествоПодъездов) > 0)
                ИзмененоДомов.Text = "Добавлено домов: 1";
            else
                ИзмененоДомов.Text = "Дом не добавлен";

            ВывестиДома_Click(sender, e);
        }

        private void УдалитьДом_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "deleteHome";

            cmd.Parameters.Add("@Адрес", SqlDbType.NVarChar, 250).Value = АдресДома.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                ИзмененоДомов.Text = "Удалено домов: " + result.ToString();
            else
                ИзмененоДомов.Text = "Дом не удален";
            con.Close();
            ВывестиДома_Click(sender, e);
        }

        private void СписокДомов_MouseClick(object sender, MouseEventArgs e)
        {   if (СписокДомов.SelectedItem != null)
            {
                АдресДома.Text = СписокДомов.SelectedItem.ToString().Substring(0, 25).Trim();
                КоличествоКвартир.Text = СписокДомов.SelectedItem.ToString().Substring(25, 25).Trim();
                КоличествоПодъездов.Text = СписокДомов.SelectedItem.ToString().Substring(50, 25).Trim();
            }
        }

        private void ОбновитьДом_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "updateHome";

            cmd.Parameters.Add("@Адрес", SqlDbType.NVarChar, 250).Value = АдресДома.Text.ToString();
            cmd.Parameters.Add("@КоличествоКвартир", SqlDbType.Int).Value = КоличествоКвартир.Text;
            cmd.Parameters.Add("@КоличествоПодъездов", SqlDbType.Int).Value = КоличествоПодъездов.Text;

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                ИзмененоДомов.Text = "Обновлено домов: " + result.ToString();
            else
                ИзмененоДомов.Text = "Дом не обновлен";
            con.Close();
            ВывестиДома_Click(sender, e);
        }

        //----------------------------Работа с заявками --------------------------------------------

        private void ВывестиЗаявки_Click(object sender, EventArgs e)
        {
            bool всеЗаявки = ВсеЗаявки.Checked ? true : false;
            bool толькоОткрытые = ТолькоОткрытые.Checked ? true : false;
            bool толькоЗакрытые = ТолькоЗакрытые.Checked ? true : false;

            IssuesOperations.вывестиЗаявки(всеЗаявки, толькоОткрытые, толькоЗакрытые);

            вывестиЗаявки();
            
        }

        private void НайтиЗаявку_Click(object sender, EventArgs e)
        {
            bool всеЗаявки = ВсеЗаявки.Checked ? true : false;
            bool толькоОткрытые = ТолькоОткрытые.Checked ? true : false;
            bool толькоЗакрытые = ТолькоЗакрытые.Checked ? true : false;

            if (НомерЗаявки.Text != "") { 
                IssuesOperations.вывестиЗаявки(всеЗаявки, толькоОткрытые, толькоЗакрытые, true, Int16.Parse(НомерЗаявки.Text));
            }
            вывестиЗаявки();
        }

        private void ДобавитьЗаявку_Click(object sender, EventArgs e)
        {

            IssuesOperations.добавитьЗаявку(ТипЗаявки.Text.ToString(), Регистратор.Text.ToString(),
                Заявитель.Text.ToString(), Примечание.Text.ToString());

            ВывестиЗаявки_Click(sender, e);
        }

        private void УдалитьЗаявку_Click(object sender, EventArgs e)
        {
            IssuesOperations.удалитьЗаявку(НомерЗаявки.Text.ToString());

            ВывестиЗаявки_Click(sender, e);
        }

        private void ОбновитьЗаявку_Click(object sender, EventArgs e)
        {
            IssuesOperations.обновитьЗаявку(НомерЗаявкиОбновление.Text, СтатусЗаявки.Text.ToString(), 
                Исполнитель.Text.ToString(), ПримечаниеОбновление.Text.ToString());

            ВывестиЗаявки_Click(sender, e);
            ВывестиИсториюЗаявкиОбновление();
        }

        private void вывестиЗаявки()
        {
            string listFormat = "{0,-7}{1,-25}{2,-18}{3,-12}{4,-30}{5,-30}{6,-30}{7,-25}{8,-35}{9,-20}{10,-180}";
            СписокЗаявок.Items.Clear();
            СписокЗаявокОбновление.Items.Clear();
            СписокЗаявок.Items.Add(String.Format(listFormat, "Номер", "Тип", "Дата создания", "Статус", "Регистратор",
                "Заявитель", "Исполнитель", "Дата закрытия", "Адрес", "Номер квартиры", "Примечание"));
            СписокЗаявокОбновление.Items.Add(String.Format(listFormat, "Номер", "Тип", "Дата создания", "Статус", "Регистратор",
                "Заявитель", "Исполнитель", "Дата закрытия", "Адрес", "Номер квартиры", "Примечание"));

            for (int i = 0; i < Issues.НомерЗаявки.Count; i++)
            {
                СписокЗаявок.Items.Add(
                    String.Format(listFormat,Issues.НомерЗаявки[i],Issues.ТипЗаявки[i],Issues.ДатаСоздания[i],
                    Issues.СтатусЗаявки[i],Issues.Регистратор[i],Issues.Заявитель[i],Issues.Исполнитель[i],
                    Issues.ДатаЗакрытия[i], Issues.Адрес[i],Issues.НомерКвартиры[i],Issues.Примечание[i]));
                СписокЗаявокОбновление.Items.Add(
                    String.Format(listFormat, Issues.НомерЗаявки[i], Issues.ТипЗаявки[i], Issues.ДатаСоздания[i],
                    Issues.СтатусЗаявки[i], Issues.Регистратор[i], Issues.Заявитель[i], Issues.Исполнитель[i],
                    Issues.ДатаЗакрытия[i], Issues.Адрес[i], Issues.НомерКвартиры[i], Issues.Примечание[i]));
            }
        }

        private void ВывестиИсториюЗаявки()
        {
            bool ВСпискеЗаявокВыбранЭлемент = (СписокЗаявок.SelectedItem != null) ? true : false;
            bool ВСпискеЗаявокВыбраноНеОглавление = (СписокЗаявок.SelectedItem != СписокЗаявок.Items[0]) ? true : false;

            IssuesHistoryOperations.вывестиИсториюЗаявки(ВСпискеЗаявокВыбранЭлемент, ВСпискеЗаявокВыбраноНеОглавление, НомерЗаявки.Text);

            string listFormat = "{0,-7}{1,-15}{2,-15}{3,-30}{4,-25}{5,-90}";
            ИсторияЗаявки.Items.Clear();
            ИсторияЗаявки.Items.Add(String.Format(listFormat, "ID", "Номер заявки", "Статус заявки", "Исполнитель", "Дата", "Примечание"));
            for (int i = 0; i < IssuesHistory.ID.Count; i++)
            {
                ИсторияЗаявки.Items.Add(
                    String.Format(listFormat, IssuesHistory.ID[i], IssuesHistory.НомерЗаявки[i], IssuesHistory.СтатусЗаявки[i],
                    IssuesHistory.Исполнитель[i], IssuesHistory.Дата[i], IssuesHistory.Примечание[i]));
            }
        }


        private void ЗаполнитьВыпадающиеМеню()
        {
            IssuesOperations.заполнитьВыпадающееМеню(ТипЗаявкиОбновление.Text.ToString());

            Регистратор.DisplayMember = "ФИО";
            Регистратор.DataSource = Issues.операторы.Tables[0].DefaultView;

            ТипЗаявки.DisplayMember = "ТипЗаявки";
            ТипЗаявки.DataSource = Issues.типыЗаявок.Tables[0].DefaultView;

            Заявитель.DisplayMember = "ФИО";
            Заявитель.DataSource = Issues.жильцы.Tables[0].DefaultView;

            СтатусЗаявки.DisplayMember = "СтатусЗаявки";
            СтатусЗаявки.DataSource = Issues.статусыЗаявок.Tables[0].DefaultView;

            if (ТипЗаявкиОбновление.Text.ToString() != "") { 
                Исполнитель.DisplayMember = "ФИО";
                Исполнитель.DataSource = Issues.исполнители.Tables[0].DefaultView;
            }
        }

        private void СписокЗаявок_MouseClick(object sender, MouseEventArgs e)
        {
            if (СписокЗаявок.SelectedItem != null)
            {
                НомерЗаявки.Text = СписокЗаявок.SelectedItem.ToString().Substring(0, 7).Trim();
                ТипЗаявки.Text = СписокЗаявок.SelectedItem.ToString().Substring(7, 25).Trim();
                Регистратор.Text = СписокЗаявок.SelectedItem.ToString().Substring(62, 30).Trim();
                Заявитель.Text = СписокЗаявок.SelectedItem.ToString().Substring(92, 30).Trim();
                ПолныйАдрес.Text = СписокЗаявок.SelectedItem.ToString().Substring(177, 35).Trim() + 
                    ", кв. " + СписокЗаявок.SelectedItem.ToString().Substring(212, 20).Trim();
                if (ПолныйАдрес.Text.ToString().Trim() == ", кв.")
                {
                    ПолныйАдрес.Text = "";
                }
                Примечание.Text = СписокЗаявок.SelectedItem.ToString().Substring(232).Trim();
                ВывестиИсториюЗаявки();
            }
        }

        

        private void ОчиститьЗаявки_Click(object sender, EventArgs e)
        {
            СписокЗаявок.Items.Clear();
            ИсторияЗаявки.Items.Clear();
            НомерЗаявки.Text = "";
            ТипЗаявки.Text = "";
            Регистратор.Text = "";
            Заявитель.Text = "";
            ПолныйАдрес.Text = "";
            Примечание.Text = "";
            
        }
        
        

        private void СписокЗаявокОбновление_MouseClick(object sender, MouseEventArgs e)
        {
            if ((СписокЗаявокОбновление.SelectedItem != null) & (СписокЗаявокОбновление.SelectedItem != СписокЗаявокОбновление.Items[0])) {
            
                НомерЗаявкиОбновление.Text = СписокЗаявокОбновление.SelectedItem.ToString().Substring(0, 7).Trim();
                ТипЗаявкиОбновление.Text = СписокЗаявокОбновление.SelectedItem.ToString().Substring(7, 25).Trim();
                Исполнитель.Text = СписокЗаявокОбновление.SelectedItem.ToString().Substring(92, 30).Trim();
                ЗаявительОбновление.Text = СписокЗаявокОбновление.SelectedItem.ToString().Substring(92, 30).Trim();
                АдресОбновление.Text = СписокЗаявокОбновление.SelectedItem.ToString().Substring(177, 35).Trim() +
                    ", кв. " + СписокЗаявокОбновление.SelectedItem.ToString().Substring(212, 20).Trim();
                if (АдресОбновление.Text.ToString().Trim() == ", кв.")
                {
                    АдресОбновление.Text = "";
                }
                ПримечаниеОбновление.Text = СписокЗаявокОбновление.SelectedItem.ToString().Substring(232).Trim();
                ЗаполнитьВыпадающиеМеню();
                СтатусЗаявки.Text = СписокЗаявокОбновление.SelectedItem.ToString().Substring(50, 12).Trim();
                ВывестиИсториюЗаявкиОбновление();
            }
        }

        private void ВывестиИсториюЗаявкиОбновление()
        {
            bool ВСпискеЗаявокВыбранЭлемент = (СписокЗаявокОбновление.SelectedItem != null) ? true : false;
            bool ВСпискеЗаявокВыбраноНеОглавление = (СписокЗаявокОбновление.SelectedItem != СписокЗаявокОбновление.Items[0]) ? true : false;

            IssuesHistoryOperations.вывестиИсториюЗаявки(true, ВСпискеЗаявокВыбраноНеОглавление, НомерЗаявкиОбновление.Text);

            string listFormat = "{0,-7}{1,-15}{2,-15}{3,-30}{4,-25}{5,-90}";
            ИсторияЗаявкиОбновление.Items.Clear();
            ИсторияЗаявкиОбновление.Items.Add(String.Format(listFormat, "ID", "Номер заявки", "Статус заявки", "Исполнитель", "Дата", "Примечание"));
            for (int i = 0; i < IssuesHistory.ID.Count; i++)
            {
                ИсторияЗаявкиОбновление.Items.Add(
                    String.Format(listFormat, IssuesHistory.ID[i], IssuesHistory.НомерЗаявки[i], IssuesHistory.СтатусЗаявки[i],
                    IssuesHistory.Исполнитель[i], IssuesHistory.Дата[i], IssuesHistory.Примечание[i]));


            }
            
        }


        private void ОсчиститьЗаявкиСобновления_Click(object sender, EventArgs e)
        {
            СписокЗаявокОбновление.Items.Clear();
            ИсторияЗаявкиОбновление.Items.Clear();
            НомерЗаявкиОбновление.Text = "";
            ТипЗаявкиОбновление.Text = "";
            СтатусЗаявки.Text = "";
            Исполнитель.Text = "";
            ЗаявительОбновление.Text = "";
            АдресОбновление.Text = "";
            ПримечаниеОбновление.Text = "";

        }

        

        //---------------------Работа с квартирами-------------------------------------

        private void ВывестиКвартиры_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select * from Квартира";
            SqlDataReader reader = cmd.ExecuteReader();
            string listFormat = "{0,-15}{1,-30}{2,-10}{3,-7}{4,-20}";
            СписокКвартир.Items.Clear();
            СписокКвартир.Items.Add(String.Format(listFormat, "Код квартиры", "Адрес", "Подъезд", "Этаж", "Номер квартиры"));

            while (reader.Read())
            {
                СписокКвартир.Items.Add(String.Format(listFormat,
                    reader["КодКвартиры"].ToString().Trim(),
                    reader["Адрес"].ToString().Trim(),
                    reader["Подъезд"].ToString().Trim(),
                    reader["Этаж"].ToString().Trim(),
                    reader["НомерКвартиры"].ToString().Trim()
                    ));
            }
            con.Close();
        }

        private void ДобавитьКвартиру_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "insertApart";

            cmd.Parameters.Add("@Адрес", SqlDbType.NVarChar, 250).Value = АдресКвартиры.Text.ToString();
            cmd.Parameters.Add("@Подъезд", SqlDbType.Int).Value = Подъезд.Text;
            cmd.Parameters.Add("@Этаж", SqlDbType.Int).Value = Этаж.Text;
            cmd.Parameters.Add("@НомерКвартиры", SqlDbType.Int).Value = НомерКвартиры.Text;

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                ИзмененоКвартир.Text = "Добавлено квартир: " + result.ToString();
            else
                ИзмененоКвартир.Text = "Квартира не добавлена";
            con.Close();
            ВывестиКвартиры_Click(sender, e);
        }

        private void ЗаполнитьComboBoxВКвартирах()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            DataSet ds = new DataSet();
            

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select Адрес from Дом";

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            АдресКвартиры.DisplayMember = "Адрес";
            АдресКвартиры.DataSource = ds.Tables[0].DefaultView;


            
            con.Close();

        }

        private void СписокКвартир_MouseClick(object sender, MouseEventArgs e)
        {
            if (СписокКвартир.SelectedItem != null)
            {
                КодКвартиры.Text = СписокКвартир.SelectedItem.ToString().Substring(0, 15).Trim();
                АдресКвартиры.Text = СписокКвартир.SelectedItem.ToString().Substring(15, 30).Trim();
                Подъезд.Text = СписокКвартир.SelectedItem.ToString().Substring(45, 10).Trim();
                Этаж.Text = СписокКвартир.SelectedItem.ToString().Substring(55, 7).Trim();
                НомерКвартиры.Text = СписокКвартир.SelectedItem.ToString().Substring(62).Trim();
            }
        }

        private void ОбновитьКвартиру_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "updateApart";

            cmd.Parameters.Add("@КодКвартиры", SqlDbType.Int).Value = КодКвартиры.Text.ToString();
            cmd.Parameters.Add("@Адрес", SqlDbType.NVarChar, 250).Value = АдресКвартиры.Text.ToString();
            cmd.Parameters.Add("@Подъезд", SqlDbType.Int).Value = Подъезд.Text;
            cmd.Parameters.Add("@Этаж", SqlDbType.Int).Value = Этаж.Text;
            cmd.Parameters.Add("@НомерКвартиры", SqlDbType.Int).Value = НомерКвартиры.Text;

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                ИзмененоКвартир.Text = "Обновлено квартир: " + result.ToString();
            else
                ИзмененоКвартир.Text = "Квартира не обновлена";
            con.Close();
            ВывестиКвартиры_Click(sender, e);
        }

        private void УдалитьКвартиру_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "deleteApart";

            cmd.Parameters.Add("@КодКвартиры", SqlDbType.Int).Value = КодКвартиры.Text.ToString();
            
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
                ИзмененоКвартир.Text = "Удалено квартир: " + result.ToString();
            else
                ИзмененоКвартир.Text = "Квартира не удалена";
            con.Close();
            ВывестиКвартиры_Click(sender, e);
        }

        private void ОчиститьКвартиры_Click(object sender, EventArgs e)
        {
            СписокКвартир.Items.Clear();
            КодКвартиры.Text = "";
            АдресКвартиры.Text = "";
            Подъезд.Text = "";
            Этаж.Text = "";
            НомерКвартиры.Text = "";
            ИзмененоКвартир.Text = "";
        }

        private void ВывестиФизЛиц_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select * from ФизЛицоСотрудник_КвартираФизЛицо_Квартира";
            SqlDataReader reader = cmd.ExecuteReader();
            string listFormat = "{0,-35}{1,-20}{2,-20}{3,-25}{4,-15}{5,-20}{6,-20}{7,-20}{8,-25}{9,-15}{10,-15}{11,-20}";
            СписокФизЛиц.Items.Clear();
            СписокФизЛиц.Items.Add(String.Format(listFormat, "ФИО", "Паспорт", "Телефон", "Должность", 
                "Ставка", "Стаж", "email", "Рабочий телефон", "Адрес", "Номер квартиры", "Код квартиры", "Тип связи"));

            while (reader.Read())
            {
                string strСтаж;
                if (reader["Стаж"].ToString().Trim() == "") { 
                     strСтаж = reader["Стаж"].ToString().Trim();
                    }
                else  strСтаж = reader["Стаж"].ToString().Substring(0, 10).Trim();

                СписокФизЛиц.Items.Add(String.Format(listFormat,
                    reader["ФИО"].ToString().Trim(),
                    reader["Паспорт"].ToString().Trim(),
                    reader["Телефон"].ToString().Trim(),
                    reader["Должность"].ToString().Trim(),
                    reader["Ставка"].ToString().Trim(),
                    strСтаж,
                    reader["email"].ToString().Trim(),
                    reader["РабочийТелефон"].ToString().Trim(),
                    reader["Адрес"].ToString().Trim(),
                    reader["НомерКвартиры"].ToString().Trim(),
                    reader["КодКвартиры"].ToString().Trim(),
                    reader["ТипСвязи"].ToString().Trim()
                    ));
            }
            con.Close();
        }

        private void СписокФизЛиц_MouseClick(object sender, MouseEventArgs e)
        {
            if (СписокФизЛиц.SelectedItem != null)
            {
                ФИО.Text = СписокФизЛиц.SelectedItem.ToString().Substring(0, 35).Trim();
                Паспорт.Text = СписокФизЛиц.SelectedItem.ToString().Substring(35, 20).Trim();
                Телефон.Text = СписокФизЛиц.SelectedItem.ToString().Substring(55, 20).Trim();
                Должность.Text = СписокФизЛиц.SelectedItem.ToString().Substring(75, 25).Trim();
                Ставка.Text = СписокФизЛиц.SelectedItem.ToString().Substring(100, 15).Trim();
                Стаж.Text = СписокФизЛиц.SelectedItem.ToString().Substring(115, 20).Trim();
                email.Text = СписокФизЛиц.SelectedItem.ToString().Substring(135, 20).Trim();
                РабочийТелефон.Text = СписокФизЛиц.SelectedItem.ToString().Substring(155, 20).Trim();
                АдресКвартирыФизЛица.Text = СписокФизЛиц.SelectedItem.ToString().Substring(175, 25).Trim();
                НомерКвартирыФизЛица.Text = СписокФизЛиц.SelectedItem.ToString().Substring(200, 15).Trim();
                КодКвартирыФизЛица.Text = СписокФизЛиц.SelectedItem.ToString().Substring(215, 15).Trim();
                ВидСвязи.Text = СписокФизЛиц.SelectedItem.ToString().Substring(230).Trim();

            }
        }

        private void ОчиститьФизЛиц_Click(object sender, EventArgs e)
        {
            СписокФизЛиц.Items.Clear();
            ФИО.Text = "";
            Паспорт.Text = "";
            Телефон.Text = "";
            Должность.Text = "";
            Ставка.Text = "";
            Стаж.Text = "";
            email.Text = "";
            РабочийТелефон.Text = "";
            АдресКвартирыФизЛица.Text = "";
            НомерКвартирыФизЛица.Text = "";
            КодКвартирыФизЛица.Text = "";
            ВидСвязи.Text = "";

        }

        private void ЗаполнитьComboBoxВФизЛицах()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();


            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select ФИО from ФизЛицо";

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            ФИО.DisplayMember = "ФИО";
            ФИО.DataSource = ds.Tables[0].DefaultView;

            cmd.CommandText = "select Должность from Должности";
            da.SelectCommand = cmd;
            da.Fill(ds1);
            Должность.DisplayMember = "Должность";
            Должность.DataSource = ds1.Tables[0].DefaultView;

            cmd.CommandText = "select Адрес from Дом";
            da.SelectCommand = cmd;
            da.Fill(ds3);
            АдресКвартирыФизЛица.DisplayMember = "Адрес";
            АдресКвартирыФизЛица.DataSource = ds3.Tables[0].DefaultView;

            cmd.CommandText = "select КодКвартиры from Квартира";
            da.SelectCommand = cmd;
            da.Fill(ds4);
            КодКвартирыФизЛица.DisplayMember = "КодКвартиры";
            КодКвартирыФизЛица.DataSource = ds4.Tables[0].DefaultView;

            con.Close();

        }

        private void ДобавитьФизЛицо_MouseHover(object sender, EventArgs e)
        {
            label27.BackColor = Color.FromArgb(255, 153, 182);
            label32.BackColor = Color.FromArgb(255, 153, 182);
            label31.BackColor = Color.FromArgb(255, 153, 182);
        }

        private void ДобавитьФизЛицо_MouseLeave(object sender, EventArgs e)
        {
            label27.BackColor = Color.Transparent;
            label32.BackColor = Color.Transparent;
            label31.BackColor = Color.Transparent;

        }

        private void ДобавитьСотрудника_MouseHover(object sender, EventArgs e)
        {
            label27.BackColor = Color.FromArgb(255, 153, 182);
            label30.BackColor = Color.FromArgb(255, 153, 182);
            label28.BackColor = Color.FromArgb(255, 153, 182);
            label29.BackColor = Color.FromArgb(255, 153, 182);
            label34.BackColor = Color.FromArgb(255, 153, 182);

        }

        private void ДобавитьСотрудника_MouseLeave(object sender, EventArgs e)
        {
            label27.BackColor = Color.Transparent;
            label30.BackColor = Color.Transparent;
            label28.BackColor = Color.Transparent;
            label29.BackColor = Color.Transparent;
            label34.BackColor = Color.Transparent;
        }

        private void УдалитьФизЛицо_MouseHover(object sender, EventArgs e)
        {
            label27.BackColor = Color.FromArgb(255, 153, 182);
        }

        private void УдалитьФизЛицо_MouseLeave(object sender, EventArgs e)
        {
            label27.BackColor = Color.Transparent;
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            label33.BackColor = Color.FromArgb(255, 153, 182);
            label30.BackColor = Color.FromArgb(255, 153, 182);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            label33.BackColor = Color.Transparent;
            label30.BackColor = Color.Transparent;
        }

        private void ДобавитьЖильца_MouseHover(object sender, EventArgs e)
        {
            label27.BackColor = Color.FromArgb(255, 153, 182);
            label36.BackColor = Color.FromArgb(255, 153, 182);
            label38.BackColor = Color.FromArgb(255, 153, 182);
        }

        private void ДобавитьЖильца_MouseLeave(object sender, EventArgs e)
        {
            label27.BackColor = Color.Transparent;
            label36.BackColor = Color.Transparent;
            label38.BackColor = Color.Transparent;
        }

        private void ДобавитьФизЛицо_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertPerson";

            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();
            cmd.Parameters.Add("@Паспорт", SqlDbType.NVarChar, 12).Value = Паспорт.Text.ToString();
            cmd.Parameters.Add("@Телефон", SqlDbType.NVarChar, 12).Value = Телефон.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void ОбновитьФизЛицо_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "updatePerson";

            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();
            cmd.Parameters.Add("@Паспорт", SqlDbType.NVarChar, 12).Value = Паспорт.Text.ToString();
            cmd.Parameters.Add("@Телефон", SqlDbType.NVarChar, 12).Value = Телефон.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void УдалитьФизЛицо_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "deletePerson";

            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void ДобавитьДолжность_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertRole";

            cmd.Parameters.Add("@Должность", SqlDbType.NVarChar, 250).Value = Должность.Text.ToString();
            cmd.Parameters.Add("@Ставка", SqlDbType.NVarChar, 250).Value = Ставка.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ЗаполнитьComboBoxВФизЛицах();
            ФИО.Text = "";
            АдресКвартирыФизЛица.Text = "";
            КодКвартирыФизЛица.Text = "";
            Должность.Text = "Должность добавлена";
        }

        private void ДобавитьСотрудника_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertEmployee";

            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();
            cmd.Parameters.Add("@Должность", SqlDbType.NVarChar, 250).Value = Должность.Text.ToString();
            cmd.Parameters.Add("@Стаж", SqlDbType.NVarChar, 250).Value = Стаж.Text.ToString();
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = email.Text.ToString();
            cmd.Parameters.Add("@РабочийТелефон", SqlDbType.NVarChar, 12).Value = РабочийТелефон.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void ОбновитьСотрудника_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "updateEmployee";

            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();
            cmd.Parameters.Add("@Должность", SqlDbType.NVarChar, 250).Value = Должность.Text.ToString();
            cmd.Parameters.Add("@Стаж", SqlDbType.NVarChar, 250).Value = Стаж.Text.ToString();
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = email.Text.ToString();
            cmd.Parameters.Add("@РабочийТелефон", SqlDbType.NVarChar, 12).Value = РабочийТелефон.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void УдалитьСотрудника_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "deleteEmployee";

            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void ДобавитьЖильца_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertTenant";

            cmd.Parameters.Add("@КодКвартиры", SqlDbType.Int).Value = КодКвартирыФизЛица.Text.ToString();
            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();
            cmd.Parameters.Add("@ТипСвязи", SqlDbType.NVarChar, 250).Value = ВидСвязи.Text.ToString();
           
            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void ОбновитьЖильца_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "updateTenant";

            cmd.Parameters.Add("@КодКвартиры", SqlDbType.Int).Value = КодКвартирыФизЛица.Text.ToString();
            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();
            cmd.Parameters.Add("@ТипСвязи", SqlDbType.NVarChar, 250).Value = ВидСвязи.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void УдалитьЖильца_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "deleteTenant";
            
            cmd.Parameters.Add("@ФИО", SqlDbType.NVarChar, 250).Value = ФИО.Text.ToString();

            int result = cmd.ExecuteNonQuery();
            con.Close();

            ВывестиФизЛиц_Click(sender, e);
        }

        private void ВсеЗаявкиОбновление_CheckedChanged(object sender, EventArgs e)
        {
            ВсеЗаявки.Checked = true;
        }

        private void ТолькоОткрытыеОбновление_CheckedChanged(object sender, EventArgs e)
        {
            ТолькоОткрытые.Checked = true;
        }

        private void ТолькоЗакрытыеОбновление_CheckedChanged(object sender, EventArgs e)
        {
            ТолькоЗакрытые.Checked = true;
        }
    }
}
