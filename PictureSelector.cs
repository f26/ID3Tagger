using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ID3Tagger
{
    public partial class PictureSelector : UserControl
    {
        public byte[] PictureData { get; set; } = new byte[0];

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
        }

        private void UpdateImage()
        {
            if (PictureData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(PictureData))
                {
                    pictureBox.Image = Image.FromStream(ms);
                }

                labelInfo.Text = pictureBox.Image.Width.ToString() + "x" +
                    pictureBox.Image.Height.ToString() + ", " +
                    (PictureData.Length / 1024).ToString() + " kB";
            }
            else
            {
                pictureBox.Image = null;
                labelInfo.Text = "0x0, 0 kB";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.PictureData = new byte[0];
            UpdateImage();
        }
    }
}
