using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using curs_reborn.Models;
using curs_reborn.WordHelper;

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
            fillComboBoxes();
            
        }
        //Заполнение КомбоБоксов из бд
        void fillComboBoxes()
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                try
                {
                    List<string> groups = (from g in db.groups select g.title).ToList();
                    comboGroups.ItemsSource = groups;
                    List<string> years = new List<string>();
                    years = (from y in db.histories select y.academ_year).Distinct().ToList();
                    comboYear.ItemsSource = years;
                    List<int> terms = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
                    comboTerm.ItemsSource = terms;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        private void save(object sender, RoutedEventArgs e)
        {
            //Если ведомость сгенерирована
            if (DG.Items.Count > 0)
            {
                //Заполняется список ведомости из грида
                for (int i = 0; i < DG.Items.Count; i++)
                {
                    st.Add(new selectStatement_Result());
                    st[i] = (selectStatement_Result)DG.Items.GetItemAt(i);
                }
                try
                {
                    //Обозначаю теги для замены в шаблоне
                    var keys = new Dictionary<string, string>
                    {
                        {"<year>", comboYear.SelectedValue.ToString()},
                        {"<term>", comboTerm.SelectedValue.ToString()},
                        {"<group>", comboGroups.SelectedValue.ToString()}
                    };
                    //Создаётся помощник для работы Word
                    var helper = new WordHelper.WordHelper(@"Ведомость.docx");
                    //Замена тегов выше на значения из комбобоксов и заполнение таблицы в шаблоне
                    helper.Process(keys, st);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Создайте ведомость");
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
                            //Заполнение таблицы ведомости в бд
                            db.fillStatement(int.Parse(comboTerm.SelectedItem.ToString()), comboYear.SelectedItem.ToString());
                            //Заполнение грида из таблицы ведомости в бд
                            DG.ItemsSource = db.selectStatement(int.Parse(comboTerm.SelectedItem.ToString()), 
                                                                          comboYear.SelectedItem.ToString(), 
                                                                          comboGroups.SelectedItem.ToString()).ToList();
                        }
                        else MessageBox.Show("Выберите группу");
                    }
                    else MessageBox.Show("Выберите год");
                }
                else MessageBox.Show("Выберите семестр");
            }
        }
        public void open(object sender, RoutedEventArgs e)
        {
            WordHelper.WordHelper word = new WordHelper.WordHelper();
            
        }
        public void exit(object sender, RoutedEventArgs e)
        {
            main.openPage(MainWindow.pages.login);
        }
    }
}
