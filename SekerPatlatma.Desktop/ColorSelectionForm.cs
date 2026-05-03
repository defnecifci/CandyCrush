using System;
using System.Windows.Forms;

namespace SekerPatlatma.Desktop
{
    /// <summary>
    /// Renk seçme formu; soyut sınıftan (ColorSelectionBaseForm) kalıtım alır.
    /// </summary>
    public partial class ColorSelectionForm : ColorSelectionBaseForm
    {
        public ColorSelectionForm()
        {
            InitializeComponent();
        }

        private void BtnRed_Click(object sender, EventArgs e)
        {
            // Base sınıftaki SelectColor metodunu çağır
            // rengi "Red" olarak belirle
            SelectColor("Red");
        }

        private void BtnPurple_Click(object sender, EventArgs e)
        {
            SelectColor("Purple");
        }

        private void BtnGreen_Click(object sender, EventArgs e)
        {
            SelectColor("Green");
        }

        private void BtnYellow_Click(object sender, EventArgs e)
        {
            SelectColor("Yellow");
        }
    }
}
