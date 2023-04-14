
namespace ID3Tagger
{
    public partial class FormPicture : Form
    {
        public FormPicture()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public byte[] PictureData
        {
            get
            {
                return pictureSelector1.PictureData;
            }
            set
            {
                pictureSelector1.PictureData = value;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
