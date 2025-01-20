using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace bdbet
{
    public partial class Form1 : Form
    {
        private readonly string connectionString = "Data Source=DESKTOP-NEW-SERVER;Initial Catalog=MyDatabase;Integrated Security=True";

        public void RegisterUser(string username, string password, string role = "User")
        {
            string hashedPassword = HashPassword(password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Balance", 100.0);
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Succesfull");
                }
            }
        }

        public int TryLogin(string username, string password, out string role)
        {
            string hashedPassword = HashPassword(password);
            role = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return 0;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string role = "User";
            string login = textBox1.Text;
            string password = textBox2.Text;
            if (login == "admin") { role = "Admin"; }
            if (button1.Text == "Login")
            {
                int check = TryLogin(login, password, out role);
                if (check != 0 && login != "admin")
                {
                    MessageBox.Show("Good!");
                    UserForm userForm = new UserForm(check);
                    userForm.Show();
                }
                if (check != 0 && login == "admin")
                {
                    MessageBox.Show("root");
                    AdminForm adminForm = new AdminForm();
                    adminForm.Show();
                }
                else if (check == 0) { MessageBox.Show("Bad(("); }
            }
            else if (button1.Text == "Registration")
            {
                RegisterUser(login, password, role);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label4.Visible = checkBox1.Checked;
            textBox3.Visible = checkBox1.Checked;
            if (checkBox1.Checked)
            {
                button1.Text = "Registration";
            }
            else { button1.Text = "Login"; }
        }
    }
}
