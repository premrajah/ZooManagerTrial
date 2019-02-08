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
                MessageBox.Show($"ShowZoos Error {ex.Message}");
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
                MessageBox.Show($"ShowAnimals Error {ex.Message}");
            }
        }

        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal a INNER JOIN ZooAnimal za ON a.Id = za.AnimalId WHERE za.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // can be imagined like a interface to make tables usable by c#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

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
                //MessageBox.Show($"ShowAssociatedAnimals Error:  {ex.Message}");
            }
        }

        private void ShowSelectedZooInTextBox()
        {
            try
            {
                string query = "SELECT Location FROM Zoo WHERE Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // can be imagined like a interface to make tables usable by c#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable zooDataTable = new DataTable();

                    sqlDataAdapter.Fill(zooDataTable);


                    textBoxEditName.Text = zooDataTable.Rows[0]["Location"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ShowSelectedZooInTextBox Error:  {ex.Message}");
            }
        }


        private void ShowSelectedAnimalsInTextBox()
        {
            try
            {
                string query = "SELECT NAME FROM Animal WHERE Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // can be imagined like a interface to make tables usable by c#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);

                    DataTable animalDataTable = new DataTable();

                    sqlDataAdapter.Fill(animalDataTable);


                    textBoxEditName.Text = animalDataTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ShowSelectedAnimalsInTextBox Error:  {ex.Message}");
            }
        }

        
        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZooInTextBox();
        }

        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalsInTextBox();
        }

        private void AddAnimalToZooBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO ZooAnimal VALUES (@ZooId, @AnimalId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open(); // open connection


                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);

                sqlCommand.ExecuteScalar(); // 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AddAnimalToZooBtn Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close(); // close connection

                ShowZoos();
                ShowAssociatedAnimals();
            }
        }

        private void DeleteAnimalBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE from Animal WHERE Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open(); // open connection

                sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);

                sqlCommand.ExecuteScalar(); // 

            }
            catch (Exception ex)
            {
                MessageBox.Show($"DeleteAnimalBtn Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close(); // close connection

                ShowAnimals();
                ShowAssociatedAnimals();
            }
        }

        private void DeleteZooBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE from Zoo WHERE Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open(); // open connection

                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                sqlCommand.ExecuteScalar(); // 

            }
            catch (Exception ex)
            {
                MessageBox.Show($"DeleteZooBtn Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close(); // close connection

                ShowZoos();
            }
        }

        private void AddZooBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO Zoo VALUES (@Location)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open(); // open connection

                sqlCommand.Parameters.AddWithValue("@Location", textBoxEditName.Text);

                sqlCommand.ExecuteScalar(); // 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AddZooBtn Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close(); // close connection

                ShowZoos();

                textBoxEditName.Text = ""; // clear text box
            }
        }

        private void AddAnimalBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO Animal VALUES (@Name)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open(); // open connection

                sqlCommand.Parameters.AddWithValue("@Name", textBoxEditName.Text);

                sqlCommand.ExecuteScalar(); // 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AddZooBtn Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close(); // close connection

                ShowAnimals();

                textBoxEditName.Text = ""; // clear text box
            }
        }

        private void UpdateZooBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE Zoo SET Location = @Location WHERE Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open(); // open connection


                sqlCommand.Parameters.AddWithValue("@zooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", textBoxEditName.Text);

                sqlCommand.ExecuteScalar(); // 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"UpdateZooBtn Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close(); // close connection

                ShowZoos();
                textBoxEditName.Text = "";
            }
        }

        private void UpdateAnimalBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE Animal SET Name = @Name WHERE Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open(); // open connection


                sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", textBoxEditName.Text);

                sqlCommand.ExecuteScalar(); // 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"UpdateZooBtn Error: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close(); // close connection

                ShowAnimals();
                textBoxEditName.Text = "";
            }
        }

        private void RemoveAnimalBtn(object sender, RoutedEventArgs e)
        {

        }
    }
}
