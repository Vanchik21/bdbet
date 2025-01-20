using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bdbet
{
    public partial class FormRecharge : Form
    {
        public FormRecharge()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out float rechargeAmount) && rechargeAmount > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректне додатне число.", "Некоректне введення", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
