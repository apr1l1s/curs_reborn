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
            for (int i = 0; i < DG.Items.Count; i++)
            {
                st.Add(new selectStatement_Result());
                st[i] = (selectStatement_Result)DG.Items.GetItemAt(i);
            }
            try
            {
                var keys = new Dictionary<string, string>
                {
                    {"<year>", comboYear.SelectedValue.ToString()},
                    {"<term>", comboTerm.SelectedValue.ToString()},
                    {"<group>", comboGroups.SelectedValue.ToString()}
                };
                var helper = new WordHelper.WordHelper(@"C:\Users\apr1l1s\source\repos\curs_reborn\Ведомость.docx");
                helper.Process(keys,st);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                            db.fillStatement(int.Parse(comboTerm.SelectedItem.ToString()),
                                                       comboYear.SelectedItem.ToString());
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
            
        }
        public void exit(object sender, RoutedEventArgs e)
        {
            main.openPage(MainWindow.pages.login);
        }
    }
}
