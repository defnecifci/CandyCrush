namespace SekerPatlatma.Desktop
{
    partial class TopScoresForm
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView listViewTopScores;
        private System.Windows.Forms.ColumnHeader columnHeaderPlayer;
        private System.Windows.Forms.ColumnHeader columnHeaderScore;

        private System.Windows.Forms.Button btnResetScores;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TopScoresForm));
            this.listViewTopScores = new System.Windows.Forms.ListView();
            this.columnHeaderPlayer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderScore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnResetScores = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewTopScores
            // 
            this.listViewTopScores.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPlayer,
            this.columnHeaderScore});
            this.listViewTopScores.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewTopScores.HideSelection = false;
            this.listViewTopScores.Location = new System.Drawing.Point(12, 12);
            this.listViewTopScores.Name = "listViewTopScores";
            this.listViewTopScores.Size = new System.Drawing.Size(260, 200);
            this.listViewTopScores.TabIndex = 0;
            this.listViewTopScores.UseCompatibleStateImageBehavior = false;
            this.listViewTopScores.View = System.Windows.Forms.View.Details;
            this.listViewTopScores.SelectedIndexChanged += new System.EventHandler(this.listViewTopScores_SelectedIndexChanged);
            // 
            // columnHeaderPlayer
            // 
            this.columnHeaderPlayer.Text = "Oyuncu";
            // 
            // columnHeaderScore
            // 
            this.columnHeaderScore.Text = "Skor";
            // 
            // btnResetScores
            // 
            this.btnResetScores.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetScores.Location = new System.Drawing.Point(12, 220);
            this.btnResetScores.Name = "btnResetScores";
            this.btnResetScores.Size = new System.Drawing.Size(260, 30);
            this.btnResetScores.TabIndex = 1;
            this.btnResetScores.Text = "Skorları Sıfırla";
            this.btnResetScores.UseVisualStyleBackColor = true;
            this.btnResetScores.Click += new System.EventHandler(this.btnResetScores_Click);
            // 
            // TopScoresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnResetScores);
            this.Controls.Add(this.listViewTopScores);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TopScoresForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "En Yüksek Skorlar";
            this.ResumeLayout(false);

        }
    }
}