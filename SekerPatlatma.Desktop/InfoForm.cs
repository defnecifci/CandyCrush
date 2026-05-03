using System;
using System.Windows.Forms;

namespace SekerPatlatma.Desktop
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
            SetGameInfo();
        }

        private void SetGameInfo()
        {
            lblInfo.Text = "Şeker Patlatma oyununa hoş geldiniz!" +
                           "Oyunda 4 farklı renkte nesne ve özel güçlere sahip joker nesneler bulunmaktadır. Jokerler; tüm satırları veya sütunları temizleme, çevresel patlatma gibi özel yeteneklere sahiptir. Zaman sınırlı bir oyun olduğundan, hızlı düşünüp stratejik hamleler yaparak en yüksek puanı hedeflemelisiniz." +
                           "En iyi skorlarınız kaydedilecek ve başarılarınız arasında sıralama yapabileceksiniz. Eğlenceli bir deneyim için hemen oyuna başlayın!\n" +
                           "İyi eğlenceler!";
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
