using System;
using System.Collections.Generic;
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

namespace prct5laba5
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = login.Text;
            string password = passwordGDR.Password;

            if (username == "Admin" && password == "123321")
            {
                Ramka.Content = new Admin();
            }
            else if (username == "User" && password == "123321")
            {
                Ramka.Content = new User();
            }

            if (AuthenticateUser(username, password))
            {
                if (IsAdmin(username))
                {
                    Admin adminWindow = new Admin();
                    adminWindow.Show();
                }
                else
                {
                    User userWindow = new User();
                    userWindow.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Не, нельзя");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-LKI8BE4\\SQLEXPRESS;Initial Catalog=Mebel;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM UserTable WHERE Username=@Username AND Password=@Password", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                int count = (int)command.ExecuteScalar();

                if (count > 0)
                    return true;
                else
                    return false;
            }
        }

        private bool IsAdmin(string username)
        {
            using (SqlConnection connection = new SqlConnection("YourConnectionString"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM UserTable WHERE Username=@Username AND Role='Admin'", connection);
                command.Parameters.AddWithValue("@Username", username);

                int count = (int)command.ExecuteScalar();

                if (count > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}

