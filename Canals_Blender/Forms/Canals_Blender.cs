using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canals_Blender
{
    public partial class Canals_Blender : Form
    {
        private string _currentLanguage = "English";

        public Canals_Blender()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(
        Application.ExecutablePath
    );
            this.Load -= CanalsBlender_Load;
            this.Load += CanalsBlender_Load;
            LoadLanguages();
        }

        public enum Canal
        {
            Red,
            Green,
            Blue
        }

        private async void CanalsBlender_Load(object sender, EventArgs e)
        {
            if (label1 != null && string.IsNullOrWhiteSpace(label1.Text))
            {
                Version current = Assembly.GetExecutingAssembly().GetName().Version;
                label1.Text = $"v{current.Major}.{current.Minor}.{Math.Max(0, current.Build)}";
            }
            await CheckForUpdatesAsync();
        }

        private void LoadLanguages()
        {
            comboBox1.SelectedIndexChanged -= ComboBox1_SelectedIndexChanged;
            comboBox1.Items.Clear();
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";

            foreach (var langKey in Localization.Translations.Keys)
            {
                string displayName = Localization.Translations[_currentLanguage].ContainsKey($"L_{langKey}")
                    ? Localization.Translations[_currentLanguage][$"L_{langKey}"]
                    : langKey;

                comboBox1.Items.Add(new KeyValuePair<string, string>(langKey, displayName));
            }

            foreach (KeyValuePair<string, string> item in comboBox1.Items)
            {
                if (item.Key == _currentLanguage)
                {
                    comboBox1.SelectedItem = item;
                    break;
                }
            }

            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is KeyValuePair<string, string> selectedPair)
            {
                string newLang = selectedPair.Key;
                if (newLang != _currentLanguage)
                {
                    _currentLanguage = newLang;
                    ApplyTranslations(_currentLanguage);
                    LoadLanguages();
                }
            }
        }

        private void ApplyTranslations(string lang)
        {
            if (Localization.Translations.ContainsKey(lang))
            {
                var t = Localization.Translations[lang];

                if (t.ContainsKey("WindowTitle")) this.Text = t["WindowTitle"];
                if (t.ContainsKey("BtnDownload")) Btn_Download.Text = t["BtnDownload"];
                if (t.ContainsKey("BtnDelete")) Btn_Delete.Text = t["BtnDelete"];
            }
        }

        private string GetText(string key, string defaultText = "")
        {
            if (Localization.Translations.ContainsKey(_currentLanguage) &&
                Localization.Translations[_currentLanguage].ContainsKey(key))
            {
                return Localization.Translations[_currentLanguage][key];
            }
            return !string.IsNullOrEmpty(defaultText) ? defaultText : key;
        }

        // ================== Auto-Update ====================== //

        private const string GitHubReleaseApiUrl =
            "https://api.github.com/repos/ZNS-Dev/Canals-Blender/releases/latest";
        private const string GitHubReleasePageUrl =
            "https://github.com/ZNS-Dev/Canals-Blender/releases/latest";

        private string ExtractJsonValue(string json, string key)
        {
            Match match = Regex.Match(
                json,
                $"\"{Regex.Escape(key)}\"\\s*:\\s*\"([^\"]+)\"",
                RegexOptions.IgnoreCase);

            return match.Success ? match.Groups[1].Value : null;
        }

        private Version ExtractVersion(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            Match match = Regex.Match(input, @"\d+(\.\d+)+");
            return match.Success && Version.TryParse(match.Value, out Version parsed)
                ? parsed
                : null;
        }

        private bool IsValidReleaseUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                return false;

            const string releasePath = "/ZNS-Dev/Canals_Blender/releases";
            return uri.Scheme == Uri.UriSchemeHttps
                && uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase)
                && (uri.AbsolutePath.Equals(releasePath, StringComparison.OrdinalIgnoreCase)
                    || uri.AbsolutePath.StartsWith(releasePath + "/", StringComparison.OrdinalIgnoreCase));
        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                Version localVersion = ExtractVersion(label1?.Text)
                    ?? Assembly.GetExecutingAssembly().GetName().Version;

                if (localVersion == null)
                    return;

                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                using HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("CanalsBlender");

                using HttpResponseMessage response = await client.GetAsync(GitHubReleaseApiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[Update] GitHub returned {(int)response.StatusCode}.");
                    return;
                }

                string json = await response.Content.ReadAsStringAsync();
                string remoteTag = ExtractJsonValue(json, "tag_name");
                string remoteName = ExtractJsonValue(json, "name");
                Version remoteVersion = ExtractVersion(remoteTag) ?? ExtractVersion(remoteName);

                if (remoteVersion == null || remoteVersion <= localVersion)
                    return;

                string releaseUrl = ExtractJsonValue(json, "html_url");
                if (!IsValidReleaseUrl(releaseUrl))
                    releaseUrl = GitHubReleasePageUrl;

                MessageBox.Show(
                    this,
                    $"Une nouvelle version de Canals Blender est disponible.\n\n" +
                    $"Version actuelle : {localVersion}\n" +
                    $"Nouvelle version : {remoteVersion}\n\n" +
                    "La page de téléchargement va s'ouvrir dans votre navigateur.",
                    "Mise à jour disponible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Process.Start(new ProcessStartInfo
                {
                    FileName = releaseUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Update Error] {ex.Message}");
            }
        }

        // ======================================================== //

        private Bitmap CompileCanals(
    Bitmap redImage,
    Bitmap greenImage,
    Bitmap blueImage)
        {
            Bitmap reference = redImage ?? greenImage ?? blueImage;

            if (reference == null)
                throw new InvalidOperationException("No image loaded.");

            int width = reference.Width;
            int height = reference.Height;

            // Verify that all images have the same dimensions
            if ((redImage != null && (redImage.Width != width || redImage.Height != height)) ||
                (greenImage != null && (greenImage.Width != width || greenImage.Height != height)) ||
                (blueImage != null && (blueImage.Width != width || blueImage.Height != height)))
            {
                throw new InvalidOperationException(
                    "All RGB images must have the same dimensions.");
            }

            Bitmap result = new Bitmap(width, height);

            try
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color red = redImage?.GetPixel(x, y) ?? Color.Black;
                        Color green = greenImage?.GetPixel(x, y) ?? Color.Black;
                        Color blue = blueImage?.GetPixel(x, y) ?? Color.Black;

                        result.SetPixel(
                            x,
                            y,
                            Color.FromArgb(red.R, green.G, blue.B)
                        );
                    }
                }

                return result;
            }
            catch
            {
                result.Dispose();
                throw;
            }
        }

        private void DecompileCanals()
        {
            Bitmap originalImage = new(T_Package.Image);
            R_Picture.Image = ConvertToBlackAndWhite(GetChannel(originalImage, Canal.Red));
            G_Picture.Image = ConvertToBlackAndWhite(GetChannel(originalImage, Canal.Green));
            B_Picture.Image = ConvertToBlackAndWhite(GetChannel(originalImage, Canal.Blue));
        }

        private Bitmap GetChannel(Bitmap originalImage, Canal channel)
        {
            Bitmap bitmap = new(originalImage.Width, originalImage.Height);
            for (int i = 0; i < originalImage.Height; i++)
            {
                for (int j = 0; j < originalImage.Width; j++)
                {
                    Color pixel = originalImage.GetPixel(j, i);
                    Color color = Color.Black;
                    switch (channel)
                    {
                        case Canal.Red:
                            color = Color.FromArgb(pixel.R, pixel.R, pixel.R);
                            break;
                        case Canal.Green:
                            color = Color.FromArgb(pixel.G, pixel.G, pixel.G);
                            break;
                        case Canal.Blue:
                            color = Color.FromArgb(pixel.B, pixel.B, pixel.B);
                            break;
                    }
                    bitmap.SetPixel(j, i, color);
                }
            }
            return bitmap;
        }

        private Bitmap ConvertToBlackAndWhite(Bitmap image)
        {
            Bitmap bitmap = new(image.Width, image.Height);
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);
                    int num = (pixel.R + pixel.G + pixel.B) / 3;
                    Color color = Color.FromArgb(num, num, num);
                    bitmap.SetPixel(j, i, color);
                }
            }
            return bitmap;
        }

        private void SaveImage(PictureBox pictureBox, string title)
        {
            if (pictureBox?.Image == null)
            {
                MessageBox.Show(
                    GetText("MsgNoImage", "Please drag and drop any image to download."),
                    GetText("MsgErrorTitle", "Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            using SaveFileDialog dialog = new()
            {
                Title = $"Save {title}",
                Filter = "PNG (*.png)|*.png|TGA (*.TGA)|*.TGA",
                DefaultExt = "png",
                AddExtension = false
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                ImageFormat format = dialog.FilterIndex switch
                {
                    1 => ImageFormat.Png,
                    2 => ImageFormat.Jpeg,
                    _ => throw new NotImplementedException(),
                };

                pictureBox.Image.Save(dialog.FileName, format);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{GetText("MsgSaveError", "Save failed")}:\n{ex.Message}",
                    GetText("MsgErrorTitle", "Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async void ShowDroppedImage(
    DragEventArgs e,
    PictureBox selectedPictureBox,
    bool CompileCanal)
        {
            try
            {
                if (e.Data == null ||
                    !e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;

                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (files == null || files.Length == 0)
                    return;

                string filePath = files[0];
                string extension = Path.GetExtension(filePath).ToLowerInvariant();

                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                        break;

                    default:
                        MessageBox.Show(
                            GetText("MsgInvalidFile", "The file must be a JPEG, JPG or PNG."),
                            GetText("MsgErrorTitle", "Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation
                        );
                        return;
                }

                // Load image without keeping the original file locked
                Bitmap loadedImage;

                using (Image image = Image.FromFile(filePath))
                {
                    loadedImage = new Bitmap(image);
                }

                Image previousImage = selectedPictureBox.Image;
                selectedPictureBox.Image = loadedImage;
                previousImage?.Dispose();

                if (CompileCanal)
                {
                    // Copy images on the UI thread
                    using Bitmap red = R_Picture.Image != null
                        ? new Bitmap(R_Picture.Image) : null;

                    using Bitmap green = G_Picture.Image != null
                        ? new Bitmap(G_Picture.Image) : null;

                    using Bitmap blue = B_Picture.Image != null
                        ? new Bitmap(B_Picture.Image) : null;

                    // Process images on a background thread
                    Bitmap result = await Task.Run(
                        () => CompileCanals(red, green, blue)
                    );

                    // Update the PictureBox on the UI thread
                    Image oldPackage = T_Package.Image;
                    T_Package.Image = result;
                    oldPackage?.Dispose();
                }
                else
                {
                    DecompileCanals();
                }
            }
            catch (Exception ex)
            {
                Exception error = ex;

                while (error.InnerException != null)
                    error = error.InnerException;

                MessageBox.Show(
                    error.ToString(),
                    "Canals Blender - Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void PictureBoxs_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //Buttons DragDrop and Delete Images
        private void R_Picture_DragDrop(object sender, DragEventArgs e) => ShowDroppedImage(e, R_Picture, CompileCanal: true);
        private void G_Picture_DragDrop(object sender, DragEventArgs e) => ShowDroppedImage(e, G_Picture, CompileCanal: true);
        private void B_Picture_DragDrop(object sender, DragEventArgs e) => ShowDroppedImage(e, B_Picture, CompileCanal: true);
        private void Btn_Download_Click(object sender, EventArgs e) => SaveImage(T_Package, "Blended Image");

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            T_Package.Image?.Dispose(); T_Package.Image = null;
            R_Picture.Image?.Dispose(); R_Picture.Image = null;
            G_Picture.Image?.Dispose(); G_Picture.Image = null;
            B_Picture.Image?.Dispose(); B_Picture.Image = null;
        }
    }
}