using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SekerPatlatma.Desktop
{
    public partial class TopScoresForm : Form
    {
        private static TopScoresForm instance;
        private string scoresFilePath;

        public static TopScoresForm GetInstance(string path)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new TopScoresForm(path);
            }
            return instance;
        }

        private TopScoresForm(string path)
        {
            InitializeComponent();
            scoresFilePath = path;
            LoadScores();
        }

        // Yeni metod: Top 5 skoru güncelle
        public void UpdateScores(List<(string Name, int Score)> topScores)
        {
            listViewTopScores.Items.Clear();
            foreach (var s in topScores)
            {
                var listViewItem = new ListViewItem(new[] { s.Name, s.Score.ToString() });
                listViewTopScores.Items.Add(listViewItem);
            }
        }

        private void LoadScores()
        {
            listViewTopScores.Items.Clear();

            if (File.Exists(scoresFilePath))
            {
                var lines = File.ReadAllLines(scoresFilePath);
                List<(string Name, int Score)> scores = new List<(string, int)>();
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        string name = parts[0].Trim();
                        if (int.TryParse(parts[1].Trim(), out int score))
                        {
                            scores.Add((name, score));
                        }
                    }
                }

                // Skorları en yüksekten düşüğe sırala ve en iyi 5'i al
                var topScores = scores.OrderByDescending(s => s.Score).Take(5).ToList();

                foreach (var s in topScores)
                {
                    var listViewItem = new ListViewItem(new[] { s.Name, s.Score.ToString() });
                    listViewTopScores.Items.Add(listViewItem);
                }
            }
        }
        private void btnResetScores_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Tüm skorları silmek istediğinizden emin misiniz?",
                                                 "Onay",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Dosyayı sil veya içeriğini temizle
                    File.WriteAllText(scoresFilePath, string.Empty);
                    listViewTopScores.Items.Clear();
                    MessageBox.Show("Tüm skorlar silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Skorlar silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void AddScore(string playerName, int score)
        {
            // Sadece UI'yi güncelleyin, dosyaya yazma işlemini burada yapmayın.
            var listViewItem = new ListViewItem(new[] { playerName, score.ToString() });
            listViewTopScores.Items.Add(listViewItem);
        }

        private void listViewTopScores_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    }


