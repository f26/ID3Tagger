
namespace ID3Tagger
{
    public partial class FormBatchEdit : Form
    {
        public string Artist
        {
            get { return textBoxArtist.Text; }
            set { textBoxArtist.Text = value; }
        }
        public string Title
        {
            get { return textBoxTitle.Text; }
            set { textBoxTitle.Text = value; }
        }
        public string Album
        {
            get { return textBoxAlbum.Text; }
            set { textBoxAlbum.Text = value; }
        }
        public string Year
        {
            get { return textBoxYear.Text; }
            set { textBoxYear.Text = value; }
        }
        public string Track
        {
            get { return textBoxTrack.Text; }
            set { textBoxTrack.Text = value; }
        }
        public string Genre
        {
            get { return textBoxGenre.Text; }
            set { textBoxGenre.Text = value; }
        }
        public string Rating
        {
            get { return textBoxRating.Text; }
            set { textBoxRating.Text = value; }
        }
        public byte[] PictureData
        {
            get { return pictureSelector1.PictureData; }
            set { pictureSelector1.PictureData = value; }
        }
        public string Comment
        {
            get { return textBoxComment.Text; }
            set { textBoxComment.Text = value; }
        }

        public bool ArtistEnabled { get { return checkBoxArtist.Checked; } }
        public bool TitleEnabled { get { return checkBoxTitle.Checked; } }
        public bool AlbumEnabled { get { return checkBoxAlbum.Checked; } }
        public bool YearEnabled { get { return checkBoxYear.Checked; } }
        public bool TrackEnabled { get { return checkBoxTrack.Checked; } }
        public bool GenreEnabled { get { return checkBoxGenre.Checked; } }
        public bool RatingEnabled { get { return checkBoxRating.Checked; } }
        public bool PictureEnabled { get { return checkBoxPicture.Checked; } }
        public bool CommentEnabled { get { return checkBoxComment.Checked; } }

        public FormBatchEdit()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public FormBatchEdit(ID3Tag tag) : this()
        {
            Artist = tag.Artist;
            Title = tag.Title;
            Album = tag.Album;
            Year = tag.Year;
            Track = tag.Track;
            Genre = tag.Genre;
            Rating = tag.Rating;
            PictureData = tag.PictureData;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void checkBoxArtist_CheckedChanged(object sender, EventArgs e)
        {
            textBoxArtist.Enabled = checkBoxArtist.Checked;
        }

        private void checkBoxTitle_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTitle.Enabled = checkBoxTitle.Checked;
        }

        private void checkBoxAlbum_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAlbum.Enabled = checkBoxAlbum.Checked;
        }

        private void checkBoxYear_CheckedChanged(object sender, EventArgs e)
        {
            textBoxYear.Enabled = checkBoxYear.Checked;
        }

        private void checkBoxTrack_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTrack.Enabled = checkBoxTrack.Checked;
        }

        private void checkBoxGenre_CheckedChanged(object sender, EventArgs e)
        {
            textBoxGenre.Enabled = checkBoxGenre.Checked;
        }

        private void checkBoxRating_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRating.Enabled = checkBoxRating.Checked;
        }

        private void checkBoxComment_CheckedChanged(object sender, EventArgs e)
        {
            textBoxComment.Enabled = checkBoxComment.Checked;
        }

        private void checkBoxPicture_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPicture.Checked)
            {
                pictureSelector1.Visible = true;
                pictureSelector1.Top = checkBoxPicture.Top;
                buttonOK.Top = buttonCancel.Top = pictureSelector1.Bottom;
                SetClientSizeCore(ClientSize.Width, buttonOK.Bottom + checkBoxPicture.Left);
            }
            else
            {
                pictureSelector1.Visible = false;
                buttonOK.Top = buttonCancel.Top = checkBoxPicture.Bottom;
                SetClientSizeCore(ClientSize.Width, buttonOK.Bottom + checkBoxPicture.Left);
            }
        }
    }
}
