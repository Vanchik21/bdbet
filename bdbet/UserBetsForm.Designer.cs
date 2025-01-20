namespace bdbet
{
    partial class UserBetsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewBets = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBets)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewBets
            // 
            this.dataGridViewBets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBets.Location = new System.Drawing.Point(12, 41);
            this.dataGridViewBets.Name = "dataGridViewBets";
            this.dataGridViewBets.RowHeadersWidth = 51;
            this.dataGridViewBets.RowTemplate.Height = 24;
            this.dataGridViewBets.Size = new System.Drawing.Size(573, 313);
            this.dataGridViewBets.TabIndex = 0;
            this.dataGridViewBets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBets_CellContentClick);
            // 
            // UserBetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 366);
            this.Controls.Add(this.dataGridViewBets);
            this.Name = "UserBetsForm";
            this.Text = "UserBetsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBets)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridViewBets;
    }
}