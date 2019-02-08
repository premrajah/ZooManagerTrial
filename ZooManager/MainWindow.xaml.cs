using System;
using System.Collections.Generic;
using System.Configuration; // configuration manager
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

namespace ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Project.Properties.Settings.ConnectionString
            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.PremSqlDBConnectionString"].ConnectionString;
        }
    }
}
