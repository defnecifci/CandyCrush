using System;
using System.Windows.Forms;
using System.IO;

namespace SekerPatlatma.Desktop
{
    public partial class GirisEkrani : Form
    {
        // Singleton instance for TopScoresForm (Opsiyonel)
        private TopScoresForm topScoresForm;

        public GirisEkrani()
        {
            InitializeComponent();
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            string playerName = textBoxPlayerName.Text.Trim();

            if (string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Lütfen bir oyuncu adı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // GameForm'u başlat ve oyuncu adını geçir
            using (GameForm gameForm = new GameForm(playerName))
            {
                gameForm.ShowDialog(this);
                // Oyun formu kapandıktan sonra skor kaydedilir (GameForm içinde)
            }
        }

        private void btnViewTopScores_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenTopScoresForm();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenTopScoresForm();
        }

        private void OpenTopScoresForm()
        {
            // TopScores.txt dosyasının tam yolunu dinamik olarak belirleyin
            string scoresFilePath = GetScoresFilePath();

            // Dosya yolunun doğru olduğundan ve yazma izinlerinizin olduğundan emin olun
            if (!File.Exists(scoresFilePath))
            {
                try
                {
                    // Resources klasörünü oluşturun ve dosyayı ekleyin
                    string resourcesPath = Path.GetDirectoryName(scoresFilePath);
                    if (!Directory.Exists(resourcesPath))
                    {
                        Directory.CreateDirectory(resourcesPath);
                    }

                    File.Create(scoresFilePath).Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Skor dosyası oluşturulurken hata oluştu: {ex.Message}",
                                    "Hata",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
            }

            // TopScoresForm'u açın (Singleton pattern kullanarak)
            if (topScoresForm == null || topScoresForm.IsDisposed)
            {
                topScoresForm = TopScoresForm.GetInstance(scoresFilePath);
            }

            topScoresForm.Show();
            topScoresForm.BringToFront();
        }

        // Skor dosyasının yolunu dinamik olarak almak için metod
        private string GetScoresFilePath()
        {
            // Uygulamanın çalıştığı dizinde "Resources" klasörü olduğunu varsayıyoruz
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string resourcesPath = Path.Combine(appDirectory, "Resources");
            string scoresFilePath = Path.Combine(resourcesPath, "TopScores.txt");

            // Resources klasörünü kontrol edin ve yoksa oluşturun
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
            }

            return scoresFilePath;
        }


        // Oyun bittiğinde skoru kaydetme metodu (GameForm tarafından çağrılacak)
        public void SavePlayerScore(string playerName, int score)
        {
            string scoresFilePath = GetScoresFilePath();
            try
            {
                using (StreamWriter sw = new StreamWriter(scoresFilePath, true))
                {
                    sw.WriteLine($"{playerName}, {score}");
                }

                // Eğer TopScoresForm açık ise, skorları güncelleyin
                if (topScoresForm != null && !topScoresForm.IsDisposed)
                {
                    topScoresForm.AddScore(playerName, score);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Skor kaydedilirken hata oluştu: {ex.Message}",
                                "Hata",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            // InfoForm'u modal olarak açmak için
            using (InfoForm infoForm = new InfoForm())
            {
                infoForm.ShowDialog(this);
            }
        }
    }
}
