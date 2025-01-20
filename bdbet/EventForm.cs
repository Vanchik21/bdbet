using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace bdbet
{
    public partial class EventForm : Form
    {
        public int eventId;
        public EventForm(int eventId)
        {
            InitializeComponent();
            this.eventId = eventId;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eventName = textBox1.Text.Trim();
            DateTime eventDate = dateTimePicker1.Value;
            bool isActive = checkBox1.Checked;

            if (string.IsNullOrEmpty(eventName))
            {
                MessageBox.Show("Будь ласка, введіть назву події.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(textBox2.Text, out decimal odds) || odds <= 0)
            {
                MessageBox.Show("Будь ласка, введіть коректний коефіцієнт (позитивне десяткове число).", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveEvent(eventName, eventDate, odds, isActive, eventId);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SaveEvent(string eventName, DateTime eventDate, decimal odds, bool isActive, int eventId)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\bdbet\\bdbet\\Database1.mdf;Integrated Security=True"; 
            string query;

            if (eventId == 0)
            {
                query = "INSERT INTO Events (Name, EventDate, Odds, IsActive) VALUES (@Name, @EventDate, @Odds, @IsActive)";
            }
            else
            {
                query = "UPDATE Events SET Name = @Name, EventDate = @EventDate, Odds = @Odds, IsActive = @IsActive WHERE EventId = @EventId";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", eventName);
                command.Parameters.AddWithValue("@EventDate", eventDate);
                command.Parameters.AddWithValue("@Odds", odds);
                command.Parameters.AddWithValue("@IsActive", isActive);

                if (eventId != 0)
                {
                    command.Parameters.AddWithValue("@EventId", eventId);
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show(eventId == 0 ? "Подію успішно додано." : "Подію успішно оновлено.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Виникла помилка при збереженні події: " + ex.Message);
                }
            }
        }
    }
}
