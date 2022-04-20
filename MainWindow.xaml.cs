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
using System.Data.Entity;
using curs_reborn.Models;
using curs_reborn.Pages;
namespace curs_reborn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string titleUser 
        {
            get
            {
                if (CurrentUser.login != null)
                {
                    if (CurrentUser.access_level == 0) return "Генератор стипендионных ведомостей (Секретарь)";
                    else return "Генератор стипендионных ведомостей (Админ)";
                } else return "Генератор стипендионных ведомостей (Гость)";
            }
            set { }
            
        }
        public user CurrentUser;
        public enum pages
        {
            login, settings, statement, admin
        }
        public void openPage(pages page)
        {
            switch (page)
            {
                case pages.login:
                    frame.Content = null;
                    CurrentUser = new user();
                    this.Title = titleUser;
                    frame.Navigate(new loginPage(this));
                    break;
                case pages.admin:
                    frame.Content = null;
                    frame.Navigate(new adminPage(this));
                    break;
                case pages.statement:
                    frame.Content = null;
                    frame.Navigate(new statementPage(this));
                    break;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            frame.Navigate(new loginPage(this));
        }
        


    }
}
