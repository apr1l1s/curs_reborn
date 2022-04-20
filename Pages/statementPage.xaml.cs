using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using curs_reborn.Models;
using Word=Microsoft.Office.Interop.Word;


namespace curs_reborn.Pages
{
    /// <summary>
    /// Логика взаимодействия для statementPage.xaml
    /// </summary>
    /// 
    public partial class statementPage : Page
    {
        
        MainWindow main;
        


        public List<selectStatement_Result> st = new List<selectStatement_Result>();
        public statementPage(MainWindow _main)
        {
            InitializeComponent();
            main = _main;
            fillComboGroups();
            fillComboTerm();
            fillComboYear();
            
        }
        void fillComboGroups()
        {
            using(curs_databaseEntities db = new curs_databaseEntities())
            {
                
                List<string> groups = (from g in db.groups select g.title).ToList();
                comboGroups.ItemsSource = groups;
                
            }
            
        }
        private void save(object sender, RoutedEventArgs e)
        {
            fillSt();
            //создаём приложение
            Word.Application WordApp = new Word.Application();
            //WordApp.Visible = true;
            //создаем документ
            Word.Document DocWord = WordApp.Application.Documents.Add();
            
            //активируем его
            DocWord.Activate();
            //записываем
            object start = 0;
            object end = 0;
            Word.Range tableLocation = DocWord.Range(ref start, ref end);
            //заполняем таблицу
            if (LV.Items.Count > 0)
            {
                int maxItems = 1 + LV.Items.Count;
                tableLocation.Text = "залупайка";
                DocWord.Tables.Add(tableLocation, maxItems, 4);
                Word.Table table = DocWord.Tables[1];
                table.Borders.Enable = 1;
                table.Cell(1, 1).Range.Text = "Фамилия";
                table.Cell(1, 2).Range.Text = "Оценка";
                table.Cell(1, 3).Range.Text = "Вид";
                table.Cell(1, 4).Range.Text = "Стоимость";
                MessageBox.Show(DocWord.Name);
                for (int row = 2; row <= maxItems; row++) table.Cell(row, 1).Range.Text = st[row - 2].surname;
                for (int row = 2; row <= maxItems; row++) table.Cell(row, 2).Range.Text = st[row - 2].mark.ToString();
                for (int row = 2; row <= maxItems; row++) table.Cell(row, 3).Range.Text = st[row - 2].grant;
                for (int row = 2; row <= maxItems; row++) table.Cell(row, 4).Range.Text = st[row - 2].cost.ToString();
            }
            try
            {
                DocWord.Save();
            }
            catch { MessageBox.Show("Ошибка сохранения"); }
            
            DocWord.Close();
            WordApp.Quit();
            MessageBox.Show("Файл успешно сохранён");
        }
        void fillComboYear()
        {
            using(curs_databaseEntities db = new curs_databaseEntities())
            {
                List<string> years = new List<string>();
                years = (from y in db.histories select y.academ_year).Distinct().ToList();
                comboYear.ItemsSource = years;
            }
            
        }
        void fillComboTerm()
        {
            using(curs_databaseEntities db = new curs_databaseEntities())
            {
                List<int> terms = new List<int>() { 1, 2 };
                comboTerm.ItemsSource = terms;
            }
        }
        void fillSt()
        {
            int i = 0;
            for (i=0;i<LV.Items.Count;i++)
            {
                st.Add(new selectStatement_Result());
                st[i] = (selectStatement_Result)LV.Items.GetItemAt(i);
            }
            MessageBox.Show($"Обработано элементов: {i}");
        }
        public void fillStatement(object sender, RoutedEventArgs e)
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                if (comboTerm.SelectedItem != null)
                {
                    if (comboYear.SelectedItem != null)
                    {
                        if (comboGroups.SelectedItem != null)
                        {
                            db.fillStatement(int.Parse(comboTerm.SelectedItem.ToString()), comboYear.SelectedItem.ToString());
                            LV.ItemsSource = db.selectStatement(int.Parse(comboTerm.SelectedItem.ToString()), comboYear.SelectedItem.ToString(), comboGroups.SelectedItem.ToString()).ToList();
                        }
                        else MessageBox.Show("Выберите группу");
                    }
                    else MessageBox.Show("Выберите год");
                }
                else MessageBox.Show("Выберите семестр");
            }
        }
        public void exit(object sender, RoutedEventArgs e)
        {
            main.openPage(MainWindow.pages.login);
            
        }

    }
}
