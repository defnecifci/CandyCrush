using System.Resources;
using System.Windows.Forms;
using SekerPatlatma.Desktop.Properties;
namespace SekerPatlatma.Desktop
{
    partial class ColorSelectionForm
    {
        private System.ComponentModel.IContainer components = null;

        private Button btnRed;
        private Button btnPurple;
        private Button btnGreen;
        private Button btnYellow;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
     new System.ComponentModel.ComponentResourceManager(typeof(ColorSelectionForm));
            this.btnRed = new System.Windows.Forms.Button();
            this.btnPurple = new System.Windows.Forms.Button();
            this.btnGreen = new System.Windows.Forms.Button();
            this.btnYellow = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // 
            // btnRed
            // 
            this.btnRed.Location = new System.Drawing.Point(10, 10);
            this.btnRed.Name = "btnRed";
            this.btnRed.Size = new System.Drawing.Size(75, 23);
            this.btnRed.TabIndex = 0;
            this.btnRed.Text = "Kırmızı";
            this.btnRed.UseVisualStyleBackColor = true;
            this.btnRed.Click += new System.EventHandler(this.BtnRed_Click);

            // 
            // btnPurple
            // 
            this.btnPurple.Location = new System.Drawing.Point(10, 50);
            this.btnPurple.Name = "btnPurple";
            this.btnPurple.Size = new System.Drawing.Size(75, 23);
            this.btnPurple.TabIndex = 1;
            this.btnPurple.Text = "Mor";
            this.btnPurple.UseVisualStyleBackColor = true;
            this.btnPurple.Click += new System.EventHandler(this.BtnPurple_Click);

            // 
            // btnGreen
            // 
            this.btnGreen.Location = new System.Drawing.Point(10, 90);
            this.btnGreen.Name = "btnGreen";
            this.btnGreen.Size = new System.Drawing.Size(75, 23);
            this.btnGreen.TabIndex = 2;
            this.btnGreen.Text = "Yeşil";
            this.btnGreen.UseVisualStyleBackColor = true;
            this.btnGreen.Click += new System.EventHandler(this.BtnGreen_Click);

            // 
            // btnYellow
            // 
            this.btnYellow.Location = new System.Drawing.Point(10, 130);
            this.btnYellow.Name = "btnYellow";
            this.btnYellow.Size = new System.Drawing.Size(75, 23);
            this.btnYellow.TabIndex = 3;
            this.btnYellow.Text = "Sarı";
            this.btnYellow.UseVisualStyleBackColor = true;
            this.btnYellow.Click += new System.EventHandler(this.BtnYellow_Click);

            // 
            // ColorSelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(100, 200);
            this.Location = new System.Drawing.Point(95000,95000);
            this.Controls.Add(this.btnRed);
            this.Controls.Add(this.btnPurple);
            this.Controls.Add(this.btnGreen);
            this.Controls.Add(this.btnYellow);
            this.Name = "ColorSelectionForm";
            this.Text = "Renk Seçimi";
            this.ResumeLayout(false);
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));


        }
    }
}
