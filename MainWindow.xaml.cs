
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using curs_reborn.Models;
using curs_reborn.Pages;

namespace curs_reborn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public user CurrentUser;
        public enum pages
        {
            login, statement, admin, users, groups, subjects, grants
        }
        public void openPage(pages page)
        {
            switch (page)
            {
                case pages.login:
                    frame.Content = null;
                    CurrentUser = new user();
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
                case pages.users:
                    frame.Content = null;
                    frame.Navigate(new Pages.editPages.usersPage(this));
                    break;
                case pages.groups:
                    frame.Content = null;
                    frame.Navigate(new Pages.editPages.groupsPage(this));
                    break;
                case pages.grants:
                    frame.Content = null;
                    frame.Navigate(new Pages.editPages.grantsPage(this));
                    break;
                case pages.subjects:
                    frame.Content = null;
                    frame.Navigate(new Pages.editPages.subjectsPage(this));
                    break;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            
            frame.Navigate(new loginPage(this));
            border();
        }
        private void border()
        {
            Border border = (Border)this.Content;
            border.MouseLeftButtonDown += new MouseButtonEventHandler(layout_MouseLeftButtonDown);
        }
        void layout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void mini(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
