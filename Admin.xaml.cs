using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace prct5laba5
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>  

    public partial class Admin : Window
    {
        private readonly string BDconnection = "DESKTOP-LKI8BE4\\SQLEXPRESS";
        private readonly string name = "Shourum";

        public Admin()
        {
            InitializeComponent();
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

                adminDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void AddData()
        {
            string cxd = grd.Text;
            string v = gsa.Text;

            if (string.IsNullOrWhiteSpace(cxd) || string.IsNullOrWhiteSpace(v))
            {
                MessageBox.Show("Заполните все поля!", "Не, нельзя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(BDconnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"INSERT INTO {name} (PerviiStolbets, VtoroiStolbats) VALUES (@cxd, @v)", connection);

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные добавлены!", "Ура, победа", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при заполнении: {ex.Message}", "Не, нельзя", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void UpdateData()
        {
            string cxd = Update.Text;
            string v = vcb.Text;

            if (string.IsNullOrWhiteSpace(cxd) || string.IsNullOrWhiteSpace(v))
            {
                MessageBox.Show("Не оставляйте поля пустыми!", "Не, нельзя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(BDconnection))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"UPDATE {name} SET FirstStolbets = @UpdatedDannie, SecondStolbets = @UpdatedInformatia", connection);
                command.Parameters.AddWithValue("@UpdatedDannie", cxd);
                command.Parameters.AddWithValue("@UpdatedInformatia", v);

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
