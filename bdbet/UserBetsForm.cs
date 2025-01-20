using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace bdbet
{
    public partial class UserBetsForm : Form
    {
        public UserBetsForm()
        {
            InitializeComponent();
        }
        public void LoadUserBets(int userId)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT Id, EventId, Amount, Odds FROM Bets WHERE UserId = @UserId";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                da.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewBets.DataSource = dt; 
            }
        }
        private void dataGridViewBets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
