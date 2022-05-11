﻿
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace curs_reborn.Pages
{
    /// <summary>
    /// Логика взаимодействия для adminPage.xaml
    /// </summary>
    public partial class adminPage : Page
    {
        MainWindow mainWindow;
        public adminPage(MainWindow _main)
        {
            mainWindow = _main;
            InitializeComponent();
            fillCB();
            
        }
        public void fillCB()
        {
            List<string> tables = new List<string>() { "Пользователи","Группы","Студенты","Стипендии","Оценки","История","Предметы" };
            CB.ItemsSource = tables;

        }
        private void exit(object sender, RoutedEventArgs e)
        {
            mainWindow.openPage(MainWindow.pages.login);
        }

        private void edit(object sender, RoutedEventArgs e)
        {
            if (CB.SelectedItem != null)
            {
                switch(CB.SelectedIndex)
                {
                    case 0:
                        mainWindow.openPage(MainWindow.pages.users);
                        break;
                    case 1:
                        mainWindow.openPage(MainWindow.pages.groups);
                        break;
                    case 2:
                        mainWindow.openPage(MainWindow.pages.students);
                        break;
                    case 3:
                        mainWindow.openPage(MainWindow.pages.grants);
                        break;
                    case 6:
                        mainWindow.openPage(MainWindow.pages.subjects);
                        break;

                }
            }
            else MessageBox.Show("Выберите таблицу");
        }
    }
}
