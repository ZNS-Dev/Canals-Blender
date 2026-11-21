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
            if (!Localization.Translations.TryGetValue(lang, out var t))
                return;

            if (t.TryGetValue("WindowTitle", out var windowTitle))
                this.Text = windowTitle;

            if (t.TryGetValue("BtnDownload", out var downloadText))
                Btn_Download.Text = downloadText;

            if (t.TryGetValue("BtnDelete", out var deleteText))
            {
                Btn_Delete_R.Text = deleteText;
                Btn_Delete_B.Text = deleteText;
                Btn_Delete_G.Text = deleteText;
                Btn_Delete_RGB.Text = deleteText;
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

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                string localText = label1 != null ? label1.Text : "";
                Version localVersion = ExtractVersion(localText);

                if (localVersion == null) return;

                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                using HttpClient client = new();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Canals Blender");

                string apiUrl = "https://api.github.com/repos/ZNS-Dev/Canals-Blender/releases/latest";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode) return;

                string json = await response.Content.ReadAsStringAsync();

                string remoteTag = ExtractJsonValue(json, "tag_name");
                string remoteName = ExtractJsonValue(json, "name");
                string downloadUrl = ExtractExeUrlFromAssets(json);

                Version remoteVersion = ExtractVersion(remoteTag) ?? ExtractVersion(remoteName);

                if (remoteVersion == null) return;

                if (remoteVersion > localVersion)
                {

                    DialogResult dialog = MessageBox.Show(
                        $"A new version is available!\n\n" +
                        $"• Current version: {localVersion}\n" +
                        $"• New version: {remoteVersion}\n\n" +
                        $"Would you like to download it and restart the application?",
                        "Update Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (dialog == DialogResult.Yes)
                    {
                        if (string.IsNullOrEmpty(downloadUrl))
                        {
                            MessageBox.Show("Error: No .exe file was found in the Release.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        await ReplaceAndRestartAsync(client, downloadUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Update Error] {ex.Message}");
            }
        }

        private async Task ReplaceAndRestartAsync(HttpClient client, string downloadUrl)
        {
            try
            {
                string currentExePath = Application.ExecutablePath;
                string tempExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update_temp.exe");

                byte[] exeData = await client.GetByteArrayAsync(downloadUrl);
                File.WriteAllBytes(tempExePath, exeData);

                string cmdArgs = $"/c timeout /t 2 /nobreak > nul & move /y \"{tempExePath}\" \"{currentExePath}\" & start \"\" \"{currentExePath}\"";

                ProcessStartInfo psi = new()
                {
                    FileName = "cmd.exe",
                    Arguments = cmdArgs,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true
                };

                Process.Start(psi);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to update application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ExtractExeUrlFromAssets(string json)
        {
            Match match = Regex.Match(json, @"\""browser_download_url\""\s*:\s*\""([^\""]+\.exe)\""", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        // ======================================================== //

        private Bitmap CompileCanals(
    Bitmap redImage,
    Bitmap greenImage,
    Bitmap blueImage)
        {
            Bitmap reference = (redImage ?? greenImage ?? blueImage) ?? throw new InvalidOperationException("No image loaded.");
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

            Bitmap result = new(width, height);

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

        private async Task RecompilePackageImageAsync()
        {
            if (R_Picture.Image == null &&
                G_Picture.Image == null &&
                B_Picture.Image == null)
            {
                Image oldImage = T_Package.Image;
                T_Package.Image = null;
                oldImage?.Dispose();
                return;
            }

            using Bitmap red = R_Picture.Image != null
                ? new Bitmap(R_Picture.Image) : null;

            using Bitmap green = G_Picture.Image != null
                ? new Bitmap(G_Picture.Image) : null;

            using Bitmap blue = B_Picture.Image != null
                ? new Bitmap(B_Picture.Image) : null;

            Bitmap result = await Task.Run(
                () => CompileCanals(red, green, blue)
            );

            Image previousImage = T_Package.Image;
            T_Package.Image = result;
            previousImage?.Dispose();
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
                if (e.Data.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
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
                    Image oldImage = T_Package.Image;
                    T_Package.Image = result;
                    oldImage?.Dispose();
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
        }

        private async void Btn_Delete_R_Click(object sender, EventArgs e)
        {
            R_Picture.Image?.Dispose();
            R_Picture.Image = null;

            await RecompilePackageImageAsync();
        }

        private async void Btn_Delete_G_Click(object sender, EventArgs e)
        {
            G_Picture.Image?.Dispose();
            G_Picture.Image = null;

            await RecompilePackageImageAsync();
        }

        private async void Btn_Delete_B_Click(object sender, EventArgs e)
        {
            B_Picture.Image?.Dispose();
            B_Picture.Image = null;

            await RecompilePackageImageAsync();
        }
    }
}