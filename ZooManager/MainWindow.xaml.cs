using System.Configuration;
using System.Windows;
using System.Data.SqlClient;
using System.Data;

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

    }
}
