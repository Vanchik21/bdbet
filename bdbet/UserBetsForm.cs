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
            using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\bdbet\\bdbet\\Database1.mdf;Integrated Security=True"))
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
