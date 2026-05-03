using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekerPatlatma.Desktop
{
    public partial class GameForm : Form
    {
        private string playerName;
        private int timeLeft = 60;
        private Timer gameTimer;
        private int rows = 8;
        private int columns = 8;
        private string[,] gameBoard = new string[8, 8];
        private string[] allIcons = { "Red", "Purple", "Green", "Yellow", "Dynamite", "Helicopter", "Rocket", "Rainbow" };
        private PictureBox[,] candyBoxes = new PictureBox[8, 8];
        private PictureBox firstCandy = null;
        private int score = 0;

        // Resimleri önceden yükleyip saklamak için bir sözlük
        private Dictionary<string, Image> candyImages = new Dictionary<string, Image>();

        // Seçili noktayı takip etmek için değişken
        private Point? selectedPoint = null;

        // Oyunun duraklatılıp duraklatılmadığını takip etmek için değişken
        private bool isPaused = false;

        private bool pKeyDown = false;

        private string scoresFilePath;

        public GameForm(string name)
        {
            InitializeComponent();
            this.KeyPreview = true; // Klavye olaylarını form seviyesinde yakalamak için
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.KeyUp += new KeyEventHandler(this.GameForm_KeyUp);
            this.FormClosing += new FormClosingEventHandler(this.GameForm_FormClosing);
            playerName = name;

            LoadCandyImages(); // Resimleri yükle
            isPaused = false;

            // Skor dosyasının yolunu alın
            scoresFilePath = GetScoresFilePath();
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            lblPlayerName.Text = $"Oyuncu: {playerName}";
            InitializeBoard();

            gameTimer = new Timer();
            gameTimer.Interval = 1000; // 1 saniye
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

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

        private Image GetCandyImage(string icon)
        {
            try
            {
                switch (icon)
                {
                    case "Red":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\kalp.png");
                    case "Purple":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\mor.png");
                    case "Green":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\yesil.png");
                    case "Yellow":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\sari.png");
                    case "Dynamite":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\dinamit.png");
                    case "Helicopter":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\helikopter.png");
                    case "Rocket":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\roket.png");
                    case "Rainbow":
                        return Image.FromFile(@"C:\Users\Administrator\Desktop\Seker Patlatma\Oyun\SekerPatlatma\SekerPatlatma.Desktop\Resources\gokkusagi.png");
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Resim yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void LoadCandyImages()
        {
            // Eğer resimleri önceden yüklemek istiyorsanız, bu metodda yapabilirsiniz.
            // Ancak, yukarıdaki GetCandyImage metodu her çağrıldığında resimleri yükleyecektir.
            // Bu nedenle, bu metodu boş bırakabilir veya önbelleğe almak için resimleri burada yükleyebilirsiniz.
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Oyun bittiğinde skorunuzu kaydedin
            SavePlayerScore();
        }
        private bool isScoreSaved = false; // Flag ekleyin

        private void SavePlayerScore()
        {
            if (isScoreSaved)
                return; // Metodun ikinci kez çalışmasını engelleyin

            try
            {
                List<(string Name, int Score)> scores = new List<(string, int)>();

                if (File.Exists(scoresFilePath))
                {
                    var lines = File.ReadAllLines(scoresFilePath);
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
                }

                // Yeni skoru ekle
                scores.Add((playerName, score));

                // Skorları en yüksekten düşüğe sırala ve en iyi 5'i al
                var topScores = scores.OrderByDescending(s => s.Score).Take(5).ToList();

                // Dosyayı güncelle (eski içeriği silerek)
                using (StreamWriter sw = new StreamWriter(scoresFilePath, false))
                {
                    foreach (var s in topScores)
                    {
                        sw.WriteLine($"{s.Name}, {s.Score}");
                    }
                }

                // Eğer TopScoresForm açık ise, skorları UI'ye ekleyin
                TopScoresForm tsForm = TopScoresForm.GetInstance(scoresFilePath);
                if (tsForm != null && !tsForm.IsDisposed)
                {
                    tsForm.UpdateScores(topScores); // Yeni bir metod ekleyin
                }

                isScoreSaved = true; // Flag'ı set edin
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Skor kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateScore(int increment)
        {
            score += increment;
            // Skoru güncellemek için gerekli işlemler
            // Örneğin, bir label ile göstermek
            lblScore.Text = $"Skor: {score}";
        }

        private void InitializeBoard()
        {
            Random rand = new Random();
            int jokerCount = (rows * columns) / 30; // Yaklaşık %3 joker
            int candyCount = (rows * columns) - jokerCount;

            var allIconsList = new List<string>();

            // Joker ikonlarını ekliyoruz (ilk 4 normal, geri kalanı joker)
            for (int i = 0; i < jokerCount; i++)
            {
                allIconsList.Add(allIcons[rand.Next(4, allIcons.Length)]);
            }

            // Şeker ikonlarını ekliyoruz
            for (int i = 0; i < candyCount; i++)
            {
                allIconsList.Add(allIcons[rand.Next(0, 4)]);
            }

            // Tüm ikonları karıştırıyoruz
            var shuffledIcons = allIconsList.OrderBy(c => rand.Next()).ToList();

            int index = 0;
            int pictureBoxSize = 40;

            // Oyun alanını kurma
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    gameBoard[i, j] = shuffledIcons[index];
                    candyBoxes[i, j] = new PictureBox
                    {
                        Width = pictureBoxSize,
                        Height = pictureBoxSize,
                        Image = GetCandyImage(gameBoard[i, j]),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Tag = new Point(i, j),
                        BackColor = Color.Transparent,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    gameBoardPanel.Controls.Add(candyBoxes[i, j]);
                    candyBoxes[i, j].Location = new Point(j * pictureBoxSize, i * pictureBoxSize);
                    candyBoxes[i, j].Click -= CandyClick; // Önceki event handler'ları kaldırın
                    candyBoxes[i, j].Click += CandyClick;
                    index++;
                }
            }

            // Oyun ilk yüklenirken de patlama varsa otomatik patlat
            CheckAndHandleMatchesAsync().ConfigureAwait(false);

            // Formun odaklanmasını sağla
            this.Focus();
        }

        // Patlama kontrolü ve işlemi
        private async Task CheckAndHandleMatchesAsync()
        {
            bool chain = true;
            while (chain)
            {
                var matches = CheckForMatches();
                if (matches.Any())
                {
                    await HighlightAndClearMatches(matches);
                    chain = true;
                }
                else
                {
                    chain = false;
                }
            }
        }

        // Patlayan şekerlerin konumlarını döndüren metod
        private List<Point> CheckForMatches()
        {
            List<Point> matchedPoints = new List<Point>();
            bool hasMatch = false;

            // Yatay Kontrol
            for (int i = 0; i < rows; i++)
            {
                int j = 0;
                while (j < columns - 2)
                {
                    if (gameBoard[i, j] != null)
                    {
                        string color = gameBoard[i, j];
                        int length = 1;
                        int k = j + 1;

                        while (k < columns && gameBoard[i, k] == color)
                        {
                            length++;
                            k++;
                        }

                        if (length >= 3)
                        {
                            for (int m = j; m < j + length; m++)
                            {
                                matchedPoints.Add(new Point(i, m));
                                UpdateScore(5); // Patlayan her şeker için puan ekle
                            }
                            hasMatch = true;
                        }
                        j += length;
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            // Dikey Kontrol
            for (int j = 0; j < columns; j++)
            {
                int i = 0;
                while (i < rows - 2)
                {
                    if (gameBoard[i, j] != null)
                    {
                        string color = gameBoard[i, j];
                        int length = 1;
                        int k = i + 1;

                        while (k < rows && gameBoard[k, j] == color)
                        {
                            length++;
                            k++;
                        }

                        if (length >= 3)
                        {
                            for (int m = i; m < i + length; m++)
                            {
                                matchedPoints.Add(new Point(m, j));
                                UpdateScore(5); // Patlayan her şeker için puan ekle
                            }
                            hasMatch = true;
                        }
                        i += length;
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            if (hasMatch)
            {
                lblNotification.Text = "Patlama oldu!";
                lblNotification.ForeColor = Color.Red;
            }
            else
            {
                lblNotification.Text = "";
            }

            return matchedPoints;
        }

        // Patlayan şekerleri vurgulayıp temizleyen metod
        private async Task HighlightAndClearMatches(List<Point> matchedPoints)
        {
            // Patlayan şekerleri vurgulama
            foreach (var point in matchedPoints)
            {
                candyBoxes[point.X, point.Y].BackColor = Color.LightCoral;
            }

            // Kısa bir süre bekleme (örneğin, 500 ms)
            await Task.Delay(500);

            // Patlayan şekerleri temizleme
            foreach (var point in matchedPoints)
            {
                gameBoard[point.X, point.Y] = null;
                candyBoxes[point.X, point.Y].BackColor = Color.Transparent;
                candyBoxes[point.X, point.Y].Image = GetCandyImage(null);
            }

            // Skoru ve bildirimleri güncelleme
            DisplayBoard();

            // Boşlukları doldurma
            await FillEmptySpacesAsync();
        }

        private void DisplayBoard()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    candyBoxes[i, j].Image = GetCandyImage(gameBoard[i, j]);
                    // Eğer seçili noktaysa arka plan rengini değiştir
                    if (selectedPoint.HasValue && selectedPoint.Value.X == i && selectedPoint.Value.Y == j)
                    {
                        candyBoxes[i, j].BackColor = Color.LightBlue;
                    }
                    else
                    {
                        candyBoxes[i, j].BackColor = Color.Transparent;
                    }
                }
            }

            // Skoru güncellemek
            lblScore.Text = $"Skor: {score}";
        }

        private async Task FillEmptySpacesAsync()
        {
            Random rand = new Random();

            for (int j = 0; j < columns; j++)
            {
                List<string> columnContents = new List<string>();

                // Sütundaki mevcut şekerleri topla
                for (int i = 0; i < rows; i++)
                {
                    if (gameBoard[i, j] != null)
                    {
                        columnContents.Add(gameBoard[i, j]);
                    }
                }

                int emptySpaceCount = rows - columnContents.Count;

                // Boşlukları doldurmak için yeni şekerler ekle
                for (int i = 0; i < emptySpaceCount; i++)
                {
                    // %3 ihtimalle joker ekle, diğer durumlarda normal şeker
                    if (rand.Next(100) < 3)
                    {
                        columnContents.Insert(0, allIcons[rand.Next(4, allIcons.Length)]);
                    }
                    else
                    {
                        columnContents.Insert(0, allIcons[rand.Next(0, 4)]);
                    }
                }

                // Sütunu yeniden oluştur
                for (int i = 0; i < rows; i++)
                {
                    gameBoard[i, j] = columnContents[i];
                }
            }

            DisplayBoard();

            // Yeniden patlama olup olmadığını kontrol et
            await CheckAndHandleMatchesAsync();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                lblTime.Text = $"Kalan Süre: {timeLeft}";
            }
            else
            {
                gameTimer.Stop();
                MessageBox.Show("Zaman bitti! Oyun sona erdi.", "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        // Klavye olaylarını yakalamak için metod
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                if (!pKeyDown)
                {
                    pKeyDown = true; // Bayrağı set et
                    TogglePause();
                }
            }
            else if (!isPaused)
            {
                HandleArrowKeys(e.KeyCode);
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                pKeyDown = false; // Bayrağı sıfırla
            }
        }

        private void TogglePause()
        {
            // Oyun şu an duraklatılmadıysa -> duraklat
            if (!isPaused)
            {
                gameTimer.Stop();                // Zamanlayıcıyı durdur
                isPaused = true;                // Durumu duraklatıldı yap
                lblNotification.Text = "Oyun duraklatıldı!";
                lblNotification.ForeColor = Color.Red;
            }
            else
            {
                // Oyun şu an zaten duraklatılmışsa -> devam et
                gameTimer.Start();              // Zamanlayıcıyı yeniden başlat
                isPaused = false;               // Durumu duraklatılmadı yap
                lblNotification.Text = "Oyun devam ediyor...";
                lblNotification.ForeColor = Color.Black;
            }
        }

        private async void HandleArrowKeys(Keys key)
        {
            if (selectedPoint == null)
            {
                MessageBox.Show("Lütfen önce bir nesne seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int x = selectedPoint.Value.X;
            int y = selectedPoint.Value.Y;
            int targetX = x;
            int targetY = y;

            switch (key)
            {
                case Keys.Up:
                    targetX = x - 1;
                    break;
                case Keys.Down:
                    targetX = x + 1;
                    break;
                case Keys.Left:
                    targetY = y - 1;
                    break;
                case Keys.Right:
                    targetY = y + 1;
                    break;
                default:
                    return;
            }

            // Hedef hücrenin sınırlar içinde olup olmadığını kontrol edin
            if (targetX < 0 || targetX >= rows || targetY < 0 || targetY >= columns)
            {
                MessageBox.Show("Bu yöne hareket edemezsiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // İki hücre arasında swap yapın
            SwapCandies(x, y, targetX, targetY);

            // Eşleşme kontrolü
            var hasMatch = CheckForMatches();
            if (hasMatch.Any())
            {
                // Patlamaları vurgulayıp temizle
                await HighlightAndClearMatches(hasMatch);
            }
            else
            {
                // Eşleşme yoksa swap'ı geri al
                SwapCandies(x, y, targetX, targetY);
                lblNotification.Text = "Eşleşme yok!";
                lblNotification.ForeColor = Color.Gray;
                await Task.Delay(500);
                lblNotification.Text = "";
            }

            // Yeni seçili hücreyi güncelleyin
            selectedPoint = new Point(targetX, targetY);
            DisplayBoard();
        }

        private void SwapCandies(int x1, int y1, int x2, int y2)
        {
            string temp = gameBoard[x1, y1];
            gameBoard[x1, y1] = gameBoard[x2, y2];
            gameBoard[x2, y2] = temp;

            DisplayBoard();
        }

        // CandyClick metodunda jokerlere veya normal şekere tıklama işlemi
        private async void CandyClick(object sender, EventArgs e)
        {
            PictureBox clickedCandy = sender as PictureBox;
            if (clickedCandy == null)
                return;

            Point clickedPoint = (Point)clickedCandy.Tag;
            int x = clickedPoint.X;
            int y = clickedPoint.Y;
            string clickedIcon = gameBoard[x, y];

            if (isPaused)
            {
                return; // Oyunun duraklatıldığını unutmayın
            }

            // Eğer tıklanan şeker bir joker ise
            if (clickedIcon == "Rainbow" || clickedIcon == "Rocket" || clickedIcon == "Helicopter" || clickedIcon == "Dynamite")
            {
                // Jokerin işlevini gerçekleştir
                if (clickedIcon == "Rainbow")
                {
                    await ShowRainbowColorSelection(x, y);
                }
                else if (clickedIcon == "Rocket")
                {
                    // Burada "horizontal" olarak sabitledik; isteğe bağlı olarak "vertical" ekleyebilirsiniz
                    await ActivateRocket(x, y, "horizontal");
                }
                else if (clickedIcon == "Helicopter")
                {
                    await ActivateHelicopter(x, y);
                }
                else if (clickedIcon == "Dynamite")
                {
                    await ActivateDynamite(x, y);
                }

                return; // Jokeri kullandıktan sonra işlem tamam
            }

            // Normal şekerlere tıklanmışsa
            if (firstCandy == null)
            {
                firstCandy = clickedCandy;
                selectedPoint = clickedPoint; // Seçili noktayı güncelleyin
                clickedCandy.BackColor = Color.LightBlue; // Seçili şekerin arka plan rengini değiştirin
                this.Focus(); // Formun odaklanmasını sağla
                return;
            }
            else
            {
                Point firstPoint = (Point)firstCandy.Tag;
                int firstX = firstPoint.X;
                int firstY = firstPoint.Y;
                int secondX = x;
                int secondY = y;

                // Eğer iki şeker birbirine komşu değilse swap yapma
                if (!CanSwap(firstX, firstY, secondX, secondY))
                {
                    // Yeni seçim yap
                    firstCandy.BackColor = Color.Transparent; // Önceki seçimin arka plan rengini sıfırla
                    firstCandy = clickedCandy;
                    selectedPoint = clickedPoint;
                    clickedCandy.BackColor = Color.LightBlue; // Yeni seçili şekerin arka plan rengini değiştirin
                    this.Focus(); // Formun odaklanmasını sağla
                    return;
                }

                // Swap işlemi
                SwapCandies(firstX, firstY, secondX, secondY);

                // Eşleşme kontrolü
                var hasMatch = CheckForMatches();
                if (hasMatch.Any())
                {
                    // Patlamaları vurgulayıp temizle
                    await HighlightAndClearMatches(hasMatch);
                }
                else
                {
                    // Eşleşme yoksa swap'ı geri al
                    SwapCandies(firstX, firstY, secondX, secondY);
                    lblNotification.Text = "Eşleşme yok!";
                    lblNotification.ForeColor = Color.Gray;
                    await Task.Delay(500);
                    lblNotification.Text = "";
                }

                // Seçimi sıfırla
                firstCandy.BackColor = Color.Transparent;
                firstCandy = null;
                selectedPoint = null;

                // Formun odaklanmasını sağla
                this.Focus();
            }
        }

        private bool CanSwap(int x1, int y1, int x2, int y2)
        {
            // Yalnızca dikey veya yatay komşu olanları swap edebilelim
            return (x1 == x2 && Math.Abs(y1 - y2) == 1)
                || (y1 == y2 && Math.Abs(x1 - x2) == 1);
        }

        // Rainbow jokerine tıklanıldığında renk seçme formunu göster
        private async Task ShowRainbowColorSelection(int jokerX, int jokerY)
        {
            using (ColorSelectionForm colorForm = new ColorSelectionForm())
            {
                var result = colorForm.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    string selectedColor = colorForm.SelectedColor;
                    await ActivateRainbow(selectedColor, jokerX, jokerY);
                }
            }
        }

        // Rainbow jokerini kullanarak seçilen renkdeki tüm şekerleri patlat
        private async Task ActivateRainbow(string selectedColor, int jokerX, int jokerY)
        {
            List<Point> matchedPoints = new List<Point>();
            bool anyPatlama = false;

            // Seçilen renkteki tüm şekerleri patlat
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (gameBoard[i, j] == selectedColor)
                    {
                        matchedPoints.Add(new Point(i, j));
                        UpdateScore(15);
                        anyPatlama = true;
                    }
                }
            }

            if (anyPatlama)
            {
                // Rainbow jokerini patlatıyoruz
                matchedPoints.Add(new Point(jokerX, jokerY));
                UpdateScore(15);

                lblNotification.Text = "Gökkuşağı patladı!";
                lblNotification.ForeColor = Color.Blue;

                // Patlayan şekerleri vurgulama
                foreach (var point in matchedPoints)
                {
                    candyBoxes[point.X, point.Y].BackColor = Color.LightBlue;
                }

                await Task.Delay(500);

                // Patlayan şekerleri temizleme
                foreach (var point in matchedPoints)
                {
                    gameBoard[point.X, point.Y] = null;
                    candyBoxes[point.X, point.Y].BackColor = Color.Transparent;
                    candyBoxes[point.X, point.Y].Image = GetCandyImage(null);
                }

                // Skoru ve bildirimleri güncelleme
                DisplayBoard();

                // Boşlukları doldurma
                await FillEmptySpacesAsync();

                // Zincirleme patlama
                await CheckAndHandleMatchesAsync();

                lblNotification.Text = "";
            }
        }

        // Dynamite jokeri: Etrafındaki 8 komşuyu patlatır
        private async Task ActivateDynamite(int x, int y)
        {
            List<Point> matchedPoints = new List<Point>();
            bool anyPatlama = false;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < rows && j >= 0 && j < columns)
                    {
                        if (gameBoard[i, j] != null)
                        {
                            matchedPoints.Add(new Point(i, j));
                            UpdateScore(20);
                            anyPatlama = true;
                        }
                    }
                }
            }

            if (anyPatlama)
            {
                lblNotification.Text = "Dinamit patladı!";
                lblNotification.ForeColor = Color.Orange;

                // Patlayan şekerleri vurgulama
                foreach (var point in matchedPoints)
                {
                    candyBoxes[point.X, point.Y].BackColor = Color.Orange;
                }

                await Task.Delay(500);

                // Patlayan şekerleri temizleme
                foreach (var point in matchedPoints)
                {
                    gameBoard[point.X, point.Y] = null;
                    candyBoxes[point.X, point.Y].BackColor = Color.Transparent;
                    candyBoxes[point.X, point.Y].Image = GetCandyImage(null);
                }

                // Skoru ve bildirimleri güncelleme
                DisplayBoard();

                // Boşlukları doldurma
                await FillEmptySpacesAsync();

                // Zincirleme patlama
                await CheckAndHandleMatchesAsync();

                lblNotification.Text = "";
            }
        }

        // Rocket jokeri: Tıklanan satırdaki tüm şekerleri patlatır (yatay)
        private async Task ActivateRocket(int x, int y, string direction)
        {
            List<Point> matchedPoints = new List<Point>();
            bool anyPatlama = false;

            if (direction == "horizontal")
            {
                for (int j = 0; j < columns; j++)
                {
                    if (gameBoard[x, j] != null)
                    {
                        matchedPoints.Add(new Point(x, j));
                        UpdateScore(10);
                        anyPatlama = true;
                    }
                }
            }
            else if (direction == "vertical")
            {
                for (int i = 0; i < rows; i++)
                {
                    if (gameBoard[i, y] != null)
                    {
                        matchedPoints.Add(new Point(i, y));
                        UpdateScore(10);
                        anyPatlama = true;
                    }
                }
            }

            if (anyPatlama)
            {
                lblNotification.Text = "Roket patladı!";
                lblNotification.ForeColor = Color.Purple;

                // Patlayan şekerleri vurgulama
                foreach (var point in matchedPoints)
                {
                    candyBoxes[point.X, point.Y].BackColor = Color.Purple;
                }

                await Task.Delay(500);

                // Patlayan şekerleri temizleme
                foreach (var point in matchedPoints)
                {
                    gameBoard[point.X, point.Y] = null;
                    candyBoxes[point.X, point.Y].BackColor = Color.Transparent;
                    candyBoxes[point.X, point.Y].Image = GetCandyImage(null);
                }

                // Skoru ve bildirimleri güncelleme
                DisplayBoard();

                // Boşlukları doldurma
                await FillEmptySpacesAsync();

                // Zincirleme patlama
                await CheckAndHandleMatchesAsync();

                lblNotification.Text = "";
            }
        }

        // Helicopter jokeri: Rastgele bir hücreyi patlatır ve helikopteri kaldırır
        private async Task ActivateHelicopter(int x, int y)
        {
            // 1. Helicopter'i kaldır
            gameBoard[x, y] = null;
            candyBoxes[x, y].Image = GetCandyImage(null);
            candyBoxes[x, y].BackColor = Color.Transparent;

            // 2. Oyun alanını güncelle
            DisplayBoard();

            // 3. Rastgele bir hücre seç ve patlat
            Random rand = new Random();
            List<Point> matchedPoints = new List<Point>();
            bool anyPatlama = false;

            int attempts = 0;
            while (attempts < 100)
            {
                int randomRow = rand.Next(0, rows);
                int randomColumn = rand.Next(0, columns);

                // Patlatılacak hücreyi seç
                if (gameBoard[randomRow, randomColumn] != null && !(randomRow == x && randomColumn == y))
                {
                    matchedPoints.Add(new Point(randomRow, randomColumn));
                    UpdateScore(5);
                    anyPatlama = true;
                    break;
                }
                attempts++;
            }

            if (anyPatlama)
            {
                lblNotification.Text = "Helikopter patladı!";
                lblNotification.ForeColor = Color.Green;

                // Patlayan şekerleri vurgulama
                foreach (var point in matchedPoints)
                {
                    candyBoxes[point.X, point.Y].BackColor = Color.LightGreen;
                }

                await Task.Delay(500);

                // Patlayan şekerleri temizleme
                foreach (var point in matchedPoints)
                {
                    gameBoard[point.X, point.Y] = null;
                    candyBoxes[point.X, point.Y].BackColor = Color.Transparent;
                    candyBoxes[point.X, point.Y].Image = GetCandyImage(null);
                }

                // Skoru ve bildirimleri güncelleme
                DisplayBoard();

                // Boşlukları doldurma
                await FillEmptySpacesAsync();

                // Zincirleme patlama
                await CheckAndHandleMatchesAsync();

                lblNotification.Text = "";
            }
        }

        private void lblPlayerName_Click(object sender, EventArgs e)
        {

        }
    }
}
    