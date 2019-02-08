using System;
using System.Collections.Generic;
using System.Configuration; // configuration manager
using System.Data;
using System.Data.SqlClient;
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
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            // Project.Properties.Settings.ConnectionString
            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.PremSqlDBConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);


            ShowZoos();
            ShowAnimals();
        }

        /// <summary>
        /// Get all elemets from the Zoo table
        /// </summary>
        private void ShowZoos()
        {
            try
            {
                string query = "SELECT * FROM Zoo";

                // can be imagined like a interface to make tables usable by c#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);


                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    // Which item should be shown in the list box
                    listZoos.DisplayMemberPath = "Location";
                    // which which item from the list cox is selected
                    listZoos.SelectedValuePath = "Id";
                    // reference to the Data the listbox should populate
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: (ShowZoos) {ex.Message}");
            }
        }

        private void ShowAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal";

                // can be imagined like a interface to make tables usable by c#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);


                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    // Which item should be shown in the list box
                    listAnimals.DisplayMemberPath = "Name";
                    // which which item from the list cox is selected
                    listAnimals.SelectedValuePath = "Id";
                    // reference to the Data the listbox should populate
                    listAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: (ShowZoos) {ex.Message}");
            }
        }

        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal a INNER JOIN ZooAnimal za ON a.Id = za.AnimalId WHERE za.ZooId = @zooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // can be imagined like a interface to make tables usable by c#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@zooId", listZoos.SelectedValue);

                    DataTable zooAnimalTable = new DataTable();

                    sqlDataAdapter.Fill(zooAnimalTable);

                    // Which item should be shown in the list box
                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    // which which item from the list cox is selected
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    // reference to the Data the listbox should populate
                    listAssociatedAnimals.ItemsSource = zooAnimalTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: (ShowZoos) {ex.Message}");
            }
        }

        private void listAssociatedAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
        }

        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddAnimalToZooBtn(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteAnimalBtn(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteZooBtn(object sender, RoutedEventArgs e)
        {

        }
    }
}
