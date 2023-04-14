using BrightIdeasSoftware;
using Microsoft.VisualBasic;

namespace ID3Tagger
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            olv = new ObjectListView();
            colFilename = new OLVColumn();
            colArtist = new OLVColumn();
            colTitle = new OLVColumn();
            colAlbum = new OLVColumn();
            colYear = new OLVColumn();
            colTrack = new OLVColumn();
            colGenre = new OLVColumn();
            colRating = new OLVColumn();
            colHasPicture = new OLVColumn();
            colComment = new OLVColumn();
            colVersion = new OLVColumn();
            textBoxDirectory = new TextBox();
            label1 = new Label();
            button1 = new Button();
            buttonShowLog = new Button();
            splitContainer1 = new SplitContainer();
            richTextBoxLog = new RichTextBox();
            buttonStrip = new Button();
            buttonRefresh = new Button();
            groupBox1 = new GroupBox();
            checkBoxWriteEmptyFrames = new CheckBox();
            checkBoxStripID3v1 = new CheckBox();
            radioButtonID3v2_4 = new RadioButton();
            radioButtonID3v2_3 = new RadioButton();
            label2 = new Label();
            checkBoxAlwaysRewrite = new CheckBox();
            groupBox2 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)olv).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // olv
            // 
            olv.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            olv.CellEditUseWholeCell = false;
            olv.Columns.AddRange(new ColumnHeader[] { colFilename, colArtist, colTitle, colAlbum, colYear, colTrack, colGenre, colRating, colHasPicture, colComment, colVersion });
            olv.Dock = DockStyle.Fill;
            olv.FullRowSelect = true;
            olv.GridLines = true;
            olv.Location = new Point(0, 0);
            olv.Name = "olv";
            olv.ShowGroups = false;
            olv.Size = new Size(758, 472);
            olv.TabIndex = 0;
            olv.View = View.Details;
            olv.CellEditFinishing += olv_CellEditFinishing;
            olv.CellClick += olv_CellClick;
            olv.CellRightClick += olv_CellRightClick;
            olv.FormatRow += olv_FormatRow;
            olv.MouseClick += olv_MouseClick;
            // 
            // colFilename
            // 
            colFilename.AspectName = "Filename";
            colFilename.FillsFreeSpace = true;
            colFilename.IsEditable = false;
            colFilename.MinimumWidth = 200;
            colFilename.Text = "Filename";
            colFilename.Width = 300;
            // 
            // colArtist
            // 
            colArtist.AspectName = "Artist";
            colArtist.Text = "Artist";
            colArtist.Width = 250;
            // 
            // colTitle
            // 
            colTitle.AspectName = "Title";
            colTitle.Text = "Title";
            colTitle.Width = 250;
            // 
            // colAlbum
            // 
            colAlbum.AspectName = "Album";
            colAlbum.Text = "Album";
            colAlbum.Width = 250;
            // 
            // colYear
            // 
            colYear.AspectName = "Year";
            colYear.Text = "Year";
            colYear.Width = 45;
            // 
            // colTrack
            // 
            colTrack.AspectName = "Track";
            colTrack.Text = "Track";
            colTrack.Width = 45;
            // 
            // colGenre
            // 
            colGenre.AspectName = "Genre";
            colGenre.Text = "Genre";
            colGenre.Width = 125;
            // 
            // colRating
            // 
            colRating.AspectName = "Rating";
            colRating.Text = "Rating";
            colRating.Width = 50;
            // 
            // colHasPicture
            // 
            colHasPicture.AspectName = "PicInfo";
            colHasPicture.IsEditable = false;
            colHasPicture.Text = "Pic";
            colHasPicture.Width = 45;
            // 
            // colComment
            // 
            colComment.AspectName = "Comment";
            colComment.Text = "Comment";
            colComment.Width = 175;
            // 
            // colVersion
            // 
            colVersion.AspectName = "Version";
            colVersion.IsEditable = false;
            colVersion.Text = "Ver";
            colVersion.Width = 45;
            // 
            // textBoxDirectory
            // 
            textBoxDirectory.Location = new Point(80, 61);
            textBoxDirectory.Name = "textBoxDirectory";
            textBoxDirectory.Size = new Size(425, 23);
            textBoxDirectory.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 64);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 1;
            label1.Text = "Directory:";
            // 
            // button1
            // 
            button1.Location = new Point(395, 22);
            button1.Name = "button1";
            button1.Size = new Size(110, 23);
            button1.TabIndex = 2;
            button1.Text = "Apply";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonApply_Click;
            // 
            // buttonShowLog
            // 
            buttonShowLog.Location = new Point(17, 22);
            buttonShowLog.Name = "buttonShowLog";
            buttonShowLog.Size = new Size(110, 23);
            buttonShowLog.TabIndex = 3;
            buttonShowLog.Text = "Show Log";
            buttonShowLog.UseVisualStyleBackColor = true;
            buttonShowLog.Click += buttonShowLog_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(12, 117);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(olv);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(richTextBoxLog);
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Size = new Size(758, 472);
            splitContainer1.SplitterDistance = 236;
            splitContainer1.TabIndex = 4;
            // 
            // richTextBoxLog
            // 
            richTextBoxLog.Dock = DockStyle.Fill;
            richTextBoxLog.Location = new Point(0, 0);
            richTextBoxLog.Name = "richTextBoxLog";
            richTextBoxLog.Size = new Size(150, 46);
            richTextBoxLog.TabIndex = 0;
            richTextBoxLog.Text = "";
            // 
            // buttonStrip
            // 
            buttonStrip.Location = new Point(269, 22);
            buttonStrip.Name = "buttonStrip";
            buttonStrip.Size = new Size(110, 23);
            buttonStrip.TabIndex = 5;
            buttonStrip.Text = "Strip Tags";
            buttonStrip.UseVisualStyleBackColor = true;
            buttonStrip.Click += buttonStrip_Click;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(143, 22);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(110, 23);
            buttonRefresh.TabIndex = 6;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBoxWriteEmptyFrames);
            groupBox1.Controls.Add(checkBoxStripID3v1);
            groupBox1.Controls.Add(radioButtonID3v2_4);
            groupBox1.Controls.Add(radioButtonID3v2_3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(checkBoxAlwaysRewrite);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(421, 99);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Options";
            // 
            // checkBoxWriteEmptyFrames
            // 
            checkBoxWriteEmptyFrames.AutoSize = true;
            checkBoxWriteEmptyFrames.Location = new Point(6, 71);
            checkBoxWriteEmptyFrames.Name = "checkBoxWriteEmptyFrames";
            checkBoxWriteEmptyFrames.Size = new Size(265, 19);
            checkBoxWriteEmptyFrames.TabIndex = 13;
            checkBoxWriteEmptyFrames.Text = "Write frames even if empty (excludes picture)";
            checkBoxWriteEmptyFrames.UseVisualStyleBackColor = true;
            // 
            // checkBoxStripID3v1
            // 
            checkBoxStripID3v1.AutoSize = true;
            checkBoxStripID3v1.Checked = true;
            checkBoxStripID3v1.CheckState = CheckState.Checked;
            checkBoxStripID3v1.Location = new Point(6, 46);
            checkBoxStripID3v1.Name = "checkBoxStripID3v1";
            checkBoxStripID3v1.Size = new Size(294, 19);
            checkBoxStripID3v1.TabIndex = 12;
            checkBoxStripID3v1.Text = "Strip ID3v1 tags on write (ID3v2 always overwritten)";
            checkBoxStripID3v1.UseVisualStyleBackColor = true;
            // 
            // radioButtonID3v2_4
            // 
            radioButtonID3v2_4.AutoSize = true;
            radioButtonID3v2_4.Location = new Point(343, 67);
            radioButtonID3v2_4.Name = "radioButtonID3v2_4";
            radioButtonID3v2_4.Size = new Size(63, 19);
            radioButtonID3v2_4.TabIndex = 11;
            radioButtonID3v2_4.Text = "ID3v2.4";
            radioButtonID3v2_4.UseVisualStyleBackColor = true;
            // 
            // radioButtonID3v2_3
            // 
            radioButtonID3v2_3.AutoSize = true;
            radioButtonID3v2_3.Checked = true;
            radioButtonID3v2_3.Location = new Point(343, 43);
            radioButtonID3v2_3.Name = "radioButtonID3v2_3";
            radioButtonID3v2_3.Size = new Size(63, 19);
            radioButtonID3v2_3.TabIndex = 10;
            radioButtonID3v2_3.TabStop = true;
            radioButtonID3v2_3.Text = "ID3v2.3";
            radioButtonID3v2_3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(329, 22);
            label2.Name = "label2";
            label2.Size = new Size(77, 15);
            label2.TabIndex = 9;
            label2.Text = "Write tags as:";
            // 
            // checkBoxAlwaysRewrite
            // 
            checkBoxAlwaysRewrite.AutoSize = true;
            checkBoxAlwaysRewrite.Location = new Point(6, 21);
            checkBoxAlwaysRewrite.Name = "checkBoxAlwaysRewrite";
            checkBoxAlwaysRewrite.Size = new Size(245, 19);
            checkBoxAlwaysRewrite.TabIndex = 8;
            checkBoxAlwaysRewrite.Text = "Rewrite tags even if they haven't changed";
            checkBoxAlwaysRewrite.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(buttonRefresh);
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(buttonShowLog);
            groupBox2.Controls.Add(textBoxDirectory);
            groupBox2.Controls.Add(buttonStrip);
            groupBox2.Location = new Point(439, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(525, 99);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Commands";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1192, 607);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMain";
            Text = "ID3Tagger";
            Load += FormMain_Load;
            Resize += FormMain_Resize;
            ((System.ComponentModel.ISupportInitialize)olv).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ObjectListView olv;
        private OLVColumn colFilename, colArtist, colTitle, colAlbum, colYear, colTrack, colGenre, colRating, colHasPicture, colVersion, colComment;
        private TextBox textBoxDirectory;
        private Label label1;
        private Button button1;
        private Button buttonShowLog;
        private SplitContainer splitContainer1;
        private RichTextBox richTextBoxLog;
        private Button buttonStrip;
        private Button buttonRefresh;
        private GroupBox groupBox1;
        private RadioButton radioButtonID3v2_4;
        private RadioButton radioButtonID3v2_3;
        private Label label2;
        private CheckBox checkBoxAlwaysRewrite;
        private CheckBox checkBoxStripID3v1;
        private GroupBox groupBox2;
        private CheckBox checkBoxWriteEmptyFrames;
    }
}