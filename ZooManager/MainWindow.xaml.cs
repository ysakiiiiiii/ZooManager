using System.Configuration;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Windows.Controls;

namespace ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnectiton;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.AcideraDBConnectionString"].ConnectionString;
            
            sqlConnectiton = new SqlConnection(connectionString);

            ShowZoos();

        }

        private void ShowZoos()
        {
            try
            {

                string query = "SELECT * FROM Zoo";

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnectiton);

                using (adapter)
                {
                    DataTable zooTable = new DataTable();
                    adapter.Fill(zooTable);

                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ShowAnimalList()
        {
            try
            {

                string query = "SELECT * FROM Animal a INNER JOIN ZooAnimal za ON a.Id = za.AnimalId where za.ZooId = @ZooId ";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnectiton);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    DataTable animalTable = new DataTable();
                    adapter.Fill(animalTable);

                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           ShowAnimalList();
        }
    }
}
