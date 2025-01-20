using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace bdbet
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void LoadEvents()
        {
            using (SqlConnection conn = new SqlConnection("Server=localhost;Database=bdbet;Trusted_Connection=True;"))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT EventId, Name, EventDate, Odds, IsActive FROM Events", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading events: " + ex.Message);
                }
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Events' table. You can move, or remove it, as needed.
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventForm addEventForm = new EventForm(0);

            if (addEventForm.ShowDialog() == DialogResult.OK)
            {
                LoadEvents();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int eventId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["EventId"].Value);
                EventForm editEventForm = new EventForm(eventId);

                if (editEventForm.ShowDialog() == DialogResult.OK)
                {
                    LoadEvents();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть подію для редагування.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цю подію?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int eventId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["EventId"].Value);

                    DeleteEvent(eventId);
                    LoadEvents();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть подію для видалення.");
            }
        }

        private void DeleteEvent(int eventId)
        {
            string connectionString = "Server=localhost;Database=bdbet;Trusted_Connection=True;";
            string query = "DELETE FROM Events WHERE EventId = @EventId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventId", eventId);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Подія успішно видалена.");
                    }
                    else
                    {
                        MessageBox.Show("Подію не знайдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Виникла помилка під час видалення події: " + ex.Message);
                }
            }
        }
    }
}
