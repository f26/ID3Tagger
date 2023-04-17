
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static System.Windows.Forms.DataFormats;

namespace ID3Tagger
{
    public partial class PictureSelector : UserControl
    {
        // Backing variables
        byte[] _picData = new byte[0];

        // The data for the associated picture.  Saves a copy when being set.
        public byte[] PictureData
        {
            get { return _picData; }
            set { _picData = PictureDataOrig = value; }
        }

        // Copy of original picture data, used when shrinking picture data
        public byte[] PictureDataOrig { get; set; } = new byte[0];

        public PictureSelector()
        {
            InitializeComponent();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "JPG Files|*.jpg;*.jpeg;";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PictureData = File.ReadAllBytes(dlg.FileName);
                    UpdateImage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.ToString());
                PictureData = new byte[0];
            }
        }

        private void PictureSelector_Load(object sender, EventArgs e)
        {
            UpdateImage();
            comboBoxMaxDimension.SelectedIndex = 0;
            comboBoxQuality.SelectedIndex = 7;
        }

        private void UpdateImage()
        {
            // Is there data to update?
            if (_picData.Length == 0)
            {
                pictureBox.Image = null;
                labelInfo.Text = "0x0, 0 kB";
                return;
            }

            // If downsampling, resize and re-encode the image as appropriate, otherwise use original data
            if (checkBoxDownsample.Checked)
            {
                // Always start with original picture data
                MemoryStream ms = new MemoryStream(PictureDataOrig);
                Image img = Image.FromStream(ms);

                // Update size
                if (checkBoxResize.Checked)
                {
                    string[] parts = comboBoxMaxDimension.Text.Split("x");
                    img = ResizeImage(img, Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
                }

                // Update quality
                long quality = Convert.ToUInt32(comboBoxQuality.Text);
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ms = new MemoryStream();
                img.Save(ms, GetEncoder(ImageFormat.Jpeg), encoderParameters);

                // Save the picture data
                _picData = ms.ToArray();
            }
            else
            {
                _picData = PictureDataOrig;
            }

            // Display the picture data
            using (MemoryStream ms = new MemoryStream(_picData))
            {
                pictureBox.Image = Image.FromStream(ms);
            }
            labelInfo.Text = pictureBox.Image.Width.ToString() + "x" +
                pictureBox.Image.Height.ToString() + ", " +
                (_picData.Length / 1024).ToString() + " kB";

        }

        // https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        // https://efundies.com/csharp-save-jpg/
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            throw new Exception("Unable to get encoder");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.PictureData = new byte[0];
            UpdateImage();
        }

        private void checkBoxResize_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxResize.Visible = comboBoxMaxDimension.Visible = labelQuality.Visible = comboBoxQuality.Visible = checkBoxDownsample.Checked;
            UpdateImage();
        }

        private void checkBoxResize_CheckedChanged_1(object sender, EventArgs e)
        {
            comboBoxMaxDimension.Enabled = checkBoxResize.Checked;
            UpdateImage();
        }

        private void comboBoxQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!checkBoxDownsample.Checked) return;
            UpdateImage();
        }

        private void comboBoxMaxDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!checkBoxDownsample.Checked) return;
            UpdateImage();
        }
    }
}
