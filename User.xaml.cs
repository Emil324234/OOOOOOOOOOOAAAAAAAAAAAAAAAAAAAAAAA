using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
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

namespace prct5laba5
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Window
    {
        private readonly string BDconnection = "DESKTOP-LKI8BE4\\SQLEXPRESS";
        private string name = "Shourum";

        public User()
        {
            InitializeComponent();
        }

        public User(string name)
        {
            InitializeComponent();
            this.name = name;
            PopulateDataGrid();
        }

        private void PopulateDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(BDconnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"SELECT * FROM {name}", connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                userDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void AddData()
        {
            string dannie = DannieTextBox.Text;
            string informatia = InformatiaTextBox.Text;

            if (string.IsNullOrWhiteSpace(dannie) || string.IsNullOrWhiteSpace(informatia))
            {
                MessageBox.Show("Заполните все поля!", "Не, нельзя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(BDconnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"INSERT INTO {name} (FirstStoblets, SecondStolbets) VALUES (@Dannie, @Informatia)", connection);
                command.Parameters.AddWithValue("@Dannie", dannie);
                command.Parameters.AddWithValue("@Informatia", informatia);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не, нельзя: {ex.Message}", "Ошибка при добавлении данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void UpdateData()
        {
            string updatedDannie = UpdateDannieTextBox.Text;
            string updatedInformatia = UpdateInformatiaTextBox.Text;

            if (string.IsNullOrWhiteSpace(updatedDannie) || string.IsNullOrWhiteSpace(updatedInformatia))
            {
                MessageBox.Show("Не оставляйте поля пустыми!", "Не, нельзя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(BDconnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"UPDATE {name} SET FirstStolbets = @UpdatedDannie, SecondStolbets = @UpdatedInformatia", connection);
                command.Parameters.AddWithValue("@UpdatedDannie", updatedDannie);
                command.Parameters.AddWithValue("@UpdatedInformatia", updatedInformatia);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Данные обновлены", "Ура, победа", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не, нельзя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Не, нельзя", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Read_Click(object sender, RoutedEventArgs e)
        {
            PopulateDataGrid();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddData();
            PopulateDataGrid();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
            PopulateDataGrid();
        }
    }
}
