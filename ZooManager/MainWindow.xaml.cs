﻿using System.Configuration;
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
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.AcideraDBConnectionString"].ConnectionString;
            
            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();
            ShowAnimals();

        }
        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAnimalList();
            ShowSelectedZooInTextBox();
        }

        private void listAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }


        private void ShowSelectedZooInTextBox()
        {
            try
            {

                string query = "SELECT Location FROM Zoo WHERE Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    DataTable zooDataTable = new DataTable();
                    adapter.Fill(zooDataTable);

                    InputEntry.Text = zooDataTable.Rows[0]["Location"].ToString();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ShowSelectedAnimalInTextBox()
        {
            try
            {

                string query = "SELECT Name FROM Animal WHERE Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                    DataTable zooDataTable = new DataTable();
                    adapter.Fill(zooDataTable);

                    InputEntry.Text = zooDataTable.Rows[0]["Name"].ToString();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ShowZoos()
        {
            try
            {

                string query = "SELECT * FROM Zoo";

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);

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

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

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



        private void ShowAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal";

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlAdapter)
                {
                    DataTable allAnimals = new DataTable();

                    sqlAdapter.Fill(allAnimals);

                    listAllAnimals.DisplayMemberPath="Name";
                    listAllAnimals.SelectedValuePath="Id";
                    listAllAnimals.ItemsSource=allAnimals.DefaultView;
                }

            }catch (Exception e)
            {
                //Console.WriteLine(e.Message); 
            }
        }
        

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "DELETE FROM Zoo WHERE Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();

            }
            catch(Exception ex)  
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO Zoo VALUES (@ZooName)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooName", InputEntry.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }

        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "INSERT INTO ZooAnimal VALUES (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open(); 
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                sqlCommand.ExecuteScalar();
                            
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimalList();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "INSERT INTO Animal VALUES (@Animal)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Animal", InputEntry.Text);

                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }
        }

        private void DeleteteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM Animal WHERE Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar() ;


            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();

            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM ZooAnimal WHERE ZooId = @ZooId AND AnimalId = @AnimalId" ;
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAssociatedAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimalList();

            }

        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "UPDATE ZOO SET Location = @Location WHERE Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", InputEntry.Text);

                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "UPDATE Animal SET Name = @Name WHERE Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", InputEntry.Text);

                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }
        }

    }
}
