using System;
using System.Windows.Forms;
using SekerPatlatma.Desktop.Properties;

namespace SekerPatlatma.Desktop
{
    /// <summary>
    /// Renk seçme formu için ortak davranışları tanımlayan abstract sınıf.
    /// </summary>
    public abstract class ColorSelectionBaseForm : Form, IColorSelector
    {
        public string SelectedColor { get; private set; }

        /// <summary>
        /// Arayüzden gelen metot; form içinde rengi seçmek için kullanılır.
        /// </summary>
        /// <param name="color">Seçilen renk</param>
        public virtual void SelectColor(string color)
        {
            SelectedColor = color;
            this.DialogResult = DialogResult.OK;
            this.Close(); // Renk seçildikten sonra form kapatılır.
        }

        private void ColorSelectionBaseForm_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSelectionBaseForm));
            this.SuspendLayout();
            // 
            // ColorSelectionBaseForm
            // 
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(249, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ColorSelectionBaseForm";
            this.Load += new System.EventHandler(this.ColorSelectionBaseForm_Load);
            this.ResumeLayout(false);
        }
    }
}
