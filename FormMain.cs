using System.Diagnostics;
using System.IO;

namespace ID3Tagger
{
    public partial class FormMain : Form
    {
        ILogger _logger = Logger.GetGlobalLogger();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            FormMain_Resize(null, null);
            textBoxDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ReloadFilesFromDisk();
        }

        private void ReloadFilesFromDisk()
        {
            olv.SuspendLayout();
            olv.ClearObjects();
            Application.DoEvents();

            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(textBoxDirectory.Text);
                List<ID3Tag> tags = new List<ID3Tag>();
                foreach (FileInfo finfo in dinfo.GetFiles("*.mp3"))
                {
                    ID3Tag id3 = new ID3Tag();
                    id3.Fullname = finfo.FullName;
                    try
                    {
                        id3 = new ID3Tag(finfo.FullName);
                    }
                    catch (Exception ex) // No tag present or unsupported tag present
                    {
                        _logger.LogErr(Path.GetFileName(finfo.FullName) + " : " + ex.Message);
                    }

                    tags.Add(id3);

                    olv.SetObjects(tags);

                }
                olv.ResumeLayout();
            }
            catch (Exception ex)
            {
                _logger.LogErr("Unable to load files from disk: " + ex.Message);
            }

            _logger.Log("Refresh from disk complete");

            richTextBoxLog.Text = _logger.ToString();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            this.splitContainer1.Width = this.ClientSize.Width - this.splitContainer1.Left * 2;
            this.splitContainer1.Height = this.ClientSize.Height - this.splitContainer1.Top - this.splitContainer1.Left;
        }

        private void olv_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            if (((ID3Tag)e.Model).Modified) e.Item.BackColor = Color.Coral;
            else e.Item.BackColor = Color.Wheat;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (olv.Objects == null) return;

            buttonApply.Enabled = olv.Enabled = false;
            Application.DoEvents();

            foreach (ID3Tag tag in olv.Objects)
            {
                if (!checkBoxAlwaysRewrite.Checked && !tag.Modified) continue;

                try
                {
                    // Read the file in from disk
                    byte[] buff = File.ReadAllBytes(tag.Fullname);

                    // Strip tags already present
                    if (checkBoxStripID3v1.Checked)
                        buff = ID3Tag.StripID3v1(buff);
                    buff = ID3Tag.StripID3v2(buff);

                    // Serialize this tag and combine with file data
                    MemoryStream ms = new MemoryStream();
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        tag.WriteEmptyFrames = checkBoxWriteEmptyFrames.Checked;

                        if (radioButtonID3v2_3.Checked)
                            bw.Write(tag.ToBytes(ID3Version.ID3v2_3));
                        else
                            bw.Write(tag.ToBytes(ID3Version.ID3v2_4));
                        bw.Write(buff);
                    }

                    // Write the file to disk
                    File.WriteAllBytes(tag.Fullname, ms.ToArray());
                    _logger.Log(Path.GetFileName(tag.Fullname) + ": Tag applied");
                }
                catch (Exception ex)
                {
                    _logger.LogErr(Path.GetFileName(tag.Fullname) + ": " + ex.Message);
                }

            }

            ReloadFilesFromDisk();
            richTextBoxLog.Text = _logger.ToString();
            buttonApply.Enabled = olv.Enabled = true;
        }


        private void olv_MouseClick(object sender, MouseEventArgs e)
        {



            // Only bring up the batch editor on right click when multiple rows are selected
            if (e.Button != MouseButtons.Right || olv.SelectedIndices.Count <= 1) return;

            // Populate batch editor with first item's info
            ID3Tag firstItem = (ID3Tag)olv.SelectedObjects[0];
            FormBatchEdit f = new FormBatchEdit(firstItem);

            if (f.ShowDialog() == DialogResult.OK)
            {
                foreach (ID3Tag tag in olv.SelectedObjects)
                {
                    if (f.ArtistEnabled) tag.Artist = f.Artist;
                    if (f.TitleEnabled) tag.Title = f.Title;
                    if (f.AlbumEnabled) tag.Album = f.Album;
                    if (f.YearEnabled) tag.Year = f.Year;
                    if (f.TrackEnabled) tag.Track = f.Track;
                    if (f.GenreEnabled) tag.Genre = f.Genre;
                    if (f.RatingEnabled) tag.Rating = f.Rating;
                    if (f.PictureEnabled) tag.PictureData = f.PictureData;
                    if (f.CommentEnabled) tag.Comment = f.Comment;

                    olv.RefreshObject(tag);
                }
            }

        }


        private void olv_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            e.Cancel = true; // Do not edit the underlying property automatically, let us handle it here

            // Check for no change
            if (e.NewValue.ToString() == e.Value.ToString()) return;

            switch (e.Column.AspectName)
            {
                case "Artist": ((ID3Tag)e.RowObject).Artist = e.NewValue.ToString(); break;
                case "Title": ((ID3Tag)e.RowObject).Title = e.NewValue.ToString(); break;
                case "Album": ((ID3Tag)e.RowObject).Album = e.NewValue.ToString(); break;
                case "Year": ((ID3Tag)e.RowObject).Year = e.NewValue.ToString(); break;
                case "Track": ((ID3Tag)e.RowObject).Track = e.NewValue.ToString(); break;
                case "Genre": ((ID3Tag)e.RowObject).Genre = e.NewValue.ToString(); break;
                case "Rating": ((ID3Tag)e.RowObject).Rating = e.NewValue.ToString(); break;
                case "Comment": ((ID3Tag)e.RowObject).Comment = e.NewValue.ToString(); break;
            }

            olv.RefreshObject(e.RowObject);
        }

        private void buttonShowLog_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed;
            this.splitContainer1.SplitterDistance = this.splitContainer1.Height / 2;
        }

        private void olv_CellClick(object sender, BrightIdeasSoftware.CellClickEventArgs e)
        {
            // If user clicked on a cell in the picture column, allow them to view/edit the associated picture
            if (e.Column == colHasPicture)
            {
                FormPicture f = new FormPicture();
                f.PictureData = ((ID3Tag)e.Model).PictureData;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ((ID3Tag)e.Model).PictureData = f.PictureData;
                    olv.RefreshObject(e.Model);
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ReloadFilesFromDisk();
        }

        private void buttonStrip_Click(object sender, EventArgs e)
        {
            // Remove all ID3 tags from selected files
            if (olv.SelectedObjects == null) return;

            FormVerify f = new FormVerify();
            if (f.ShowDialog() != DialogResult.OK || olv.Objects == null) return;

            buttonApply.Enabled = olv.Enabled = false;
            Application.DoEvents();

            foreach (ID3Tag tag in olv.SelectedObjects)
            {
                try
                {
                    byte[] buff = File.ReadAllBytes(tag.Fullname);
                    buff = ID3Tag.StripID3v1(buff);
                    buff = ID3Tag.StripID3v2(buff);
                    File.WriteAllBytes(tag.Fullname, buff);
                    _logger.Log(Path.GetFileName(tag.Fullname) + ": Tags stripped");
                }
                catch (Exception ex)
                {
                    _logger.LogErr(Path.GetFileName(tag.Fullname) + ": " + ex.Message);
                }
            }

            ReloadFilesFromDisk();
            richTextBoxLog.Text = _logger.ToString();
            buttonApply.Enabled = olv.Enabled = true;
        }

        private void olv_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            // Allow user to locate a file on disk if they right click on the filename (first column)
            if (olv.SelectedItems.Count == 1 && e.Column == colFilename)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem tmi = new ToolStripMenuItem("Locate on disk");
                tmi.Click += Tmi_Click;
                tmi.Tag = olv.SelectedObject;
                cms.Items.Add(tmi);
                cms.Show(Cursor.Position);
            }
        }

        private void Tmi_Click(object sender, EventArgs e)
        {
            // Open an explorer window and hilight the appropriate file
            try
            {
                string filename = ((ID3Tag)((ToolStripMenuItem)sender).Tag).Fullname;
                string args = string.Format("/e, /select, \"{0}\"", Path.GetFullPath(filename), Path.GetFileName(filename));

                // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "explorer";
                info.Arguments = args;
                Process.Start(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonAutoTag_Click(object sender, EventArgs e)
        {
            // Remove all ID3 tags from selected files
            if (olv.SelectedObjects == null) return;

            buttonApply.Enabled = olv.Enabled = false;
            Application.DoEvents();

            foreach (ID3Tag tag in olv.SelectedObjects)
            {
                try
                {
                    string filename = Path.GetFileName(tag.Fullname);
                    string[] parts = filename.Split(" - ");

                    if (parts.Length == 2)
                    {
                        tag.Artist = parts[0];
                        tag.Title = parts[1].Replace(".mp3", "").Replace(".MP3", "");
                        olv.RefreshObject(tag);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogErr(Path.GetFileName(tag.Fullname) + ": " + ex.Message);
                }
            }

            buttonApply.Enabled = olv.Enabled = true;
        }
    }
}