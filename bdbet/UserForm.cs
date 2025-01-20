using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace bdbet
{
    public partial class UserForm : Form
    {
        private int userId;
        public UserForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            timer1.Start();
            LoadActiveEvents();
        }
        private void UserForm_Load(object sender, EventArgs e)
        {
            LoadUserData(this.userId);
        }

        private void LoadUserData(int userId)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True"))
            {
                string sql = "SELECT Username, Balance FROM Users WHERE Id = @UserId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader["Username"].ToString();
                            string balance = reader["Balance"].ToString();
                            label4.Text = balance;
                            label1.Text = name;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void LoadActiveEvents()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True"))
            {
                string query = "SELECT EventId, Name, EventDate, Odds FROM Events WHERE IsActive = 1";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void UpdateUserBalance(int userId, float amountToAdd)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True"))
            {
                string sql = "UPDATE Users SET Balance = Balance + @Amount WHERE Id = @UserId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Amount", amountToAdd);
                command.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Your balance has been updated.");
                    LoadUserData(userId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            LoadUserData(this.userId);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormRecharge rechargeForm = new FormRecharge();
            var dialogResult = rechargeForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                float rechargeAmount = float.Parse(rechargeForm.textBox1.Text);
                UpdateUserBalance(userId, rechargeAmount);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Будь ласка, виберіть подію.");
                return;
            }

            if (!decimal.TryParse(textBox1.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введіть коректну суму ставки.");
                return;
            }

            int eventId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["EventId"].Value);

            if (PlaceBet(this.userId, eventId, amount))
            {
                MessageBox.Show("Ставку розміщено успішно.");
                LoadUserData(this.userId);
                LoadActiveEvents();
            }
        }
        private bool PlaceBet(int userId, int eventId, decimal amount)
        {
            decimal currentBalance = Convert.ToDecimal(GetBalance(userId));
            if (currentBalance < amount)
            {
                MessageBox.Show("Недостатньо коштів для роблення ставки.");
                return false;
            }

            using (SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True"))
            {
                try
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE Users SET Balance = Balance - @Amount WHERE Id = @Id", conn, tran);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Id", userId);
                        cmd.ExecuteNonQuery();

                        decimal odds = GetOddsForEvent(eventId);
                        cmd = new SqlCommand("INSERT INTO Bets (UserId, EventId, Amount, Odds) VALUES (@UserId, @EventId, @Amount, @Odds)", conn, tran);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Odds", odds);
                        cmd.ExecuteNonQuery();

                        tran.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при розміщенні ставки: " + ex.Message);
                    return false;
                }
            }
        }

        private double GetBalance(int id)
        {
            double balance = 0;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Balance FROM Users WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        balance = Convert.ToDouble(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при отриманні балансу: " + ex.Message);
                }
            }

            return balance;
        }

        private decimal GetOddsForEvent(int eventId)
        {
            decimal odds = 0;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Odds FROM Events WHERE EventId = @EventId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventId", eventId);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        odds = (decimal)result;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при отриманні коефіцієнтів: " + ex.Message);
                }
            }

            return odds;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadActiveEvents();
            LoadUserData(this.userId);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UserBetsForm betsForm = new UserBetsForm();
            betsForm.LoadUserBets(this.userId);
            betsForm.ShowDialog();
        }
    }
}
