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

namespace curs_reborn.Pages.editPages
{
    /// <summary>
    /// Логика взаимодействия для grantsPage.xaml
    /// </summary>
    public partial class grantsPage : Page
    {
        MainWindow main;
        public grantsPage(MainWindow _main)
        {
            InitializeComponent();
            this.main = _main;
        }
    }
}
