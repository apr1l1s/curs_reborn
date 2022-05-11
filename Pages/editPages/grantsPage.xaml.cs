using curs_reborn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace curs_reborn.Pages.editPages
{
    /// <summary>
    /// Логика взаимодействия для grantsPage.xaml
    /// </summary>
    public partial class grantsPage : Page
    {
        MainWindow main;
        grant cng;
        public grantsPage(MainWindow _main)
        {
            InitializeComponent();
            this.main = _main;
            fillDG();
        }
        private void fillDG()
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                DG.ItemsSource = (from g in db.grants select g).ToList();
            }

        }
        private bool checkBoxes()
        {
            var type = new Regex(@"\A[А-Я]{1}[а-я]{4,49}\z");
            var year = new Regex(@"\A[0-9]{4}/[0-9]{4}\z");
            var term = new Regex(@"\A[1-8]{1}\z");
            var cost = new Regex(@"\A([0-9]{1,10}\.[0-9]{1,4})|([0-9]{1,10})\z");
            if (type.IsMatch(TypeBox.Text))
            {
                if (year.IsMatch(YearBox.Text))
                {
                    if (term.IsMatch(TermBox.Text))
                    {
                        if (cost.IsMatch(CostBox.Text))
                        {
                            return true;
                        }
                        else MessageBox.Show("Неподходящий размер");
                    }
                    else MessageBox.Show("Неподходящий семестр");
                }
                else MessageBox.Show("Неподходящий год");

            }
            else MessageBox.Show("Неподходящий тип");
            return false;
        }
        private void addOrEditUser()
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                try
                {
                    //Проверка
                    if (checkBoxes())
                    {
                        decimal i;
                        decimal.TryParse(CostBox.Text, out i);
                        int t;
                        int.TryParse(TermBox.Text, out t);
                        //Выбирали элемент для редактирования
                        if (cng != null)
                        {
                            //Да 
                            //Поиск уже существующих данных с этим номером
                            var chk = from g in db.grants
                                      where g.grant_id == cng.grant_id
                                      select g;
                            //В зависимости от этого удаление или редактирование
                            if (chk.Count() > 0)
                            {
                                // обновляем
                                db.editGrant(cng.grant_id, TypeBox.Text,i,t,YearBox.Text);
                            }
                            else
                            {
                                //добавляем
                                db.addGrant(TypeBox.Text, i, t, YearBox.Text);
                            }
                        }
                        else
                        {
                            //Добавление
                            db.addGrant(TypeBox.Text, i, t, YearBox.Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обвновления базы данных: {ex.Message}");
                }
            }
            fillDG();
            TypeBox.Text = "";
            TypeBox.IsReadOnly = true;
            TermBox.Text = "";
            TermBox.IsReadOnly = true;
            CostBox.Text = "";
            CostBox.IsReadOnly = true;
            YearBox.Text = "";
            YearBox.IsReadOnly = true;

        }
        private void save(object sender, RoutedEventArgs e)
        {
            using (curs_databaseEntities db = new curs_databaseEntities())
            {
                //При нажатии на кнопку Сохранить
                addOrEditUser();
            }
        }
        private void add(object sender, RoutedEventArgs e)
        {
            //Разрешает писать в полях и отменять выбор
            TypeBox.IsReadOnly = false;
            TermBox.IsReadOnly = false;
            CostBox.IsReadOnly = false;
            YearBox.IsReadOnly = false;
            cng = null;
            DG.SelectedIndex = -1;
        }
        private void del(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedIndex != -1)
            {
                try
                {
                    //Берётся элемент из грида
                    cng = (grant)DG.Items[DG.SelectedIndex];
                    using (curs_databaseEntities db = new curs_databaseEntities())
                    {
                        //Удаляется из бд
                        db.delGrant(cng.grant_id);
                        db.SaveChanges();
                    }
                    //Обновляется
                    fillDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления" + ex.Message);
                }

            }
        }
        private void edit(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedIndex != -1)
            {
                //Берётся элемент из грида
                cng = (grant)DG.Items[DG.SelectedIndex];
                //Все данные записываются в блоки для редактирования
                TypeBox.Text = cng.type;
                TermBox.Text = cng.term.ToString();
                YearBox.Text = cng.academ_year;
                CostBox.Text = cng.cost.ToString();
                //Пользователь может редактировать блоки
                TypeBox.IsReadOnly = false;
                CostBox.IsReadOnly = false;
                TermBox.IsReadOnly = false;
                YearBox.IsReadOnly = false;
            }
            else MessageBox.Show("Выберите элемент");
        }
        private void exit(object sender, RoutedEventArgs e)
        {
            //Возврат на страницу админа
            main.openPage(MainWindow.pages.admin);
        }
    }
}
