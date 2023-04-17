namespace ID3Tagger
{
    partial class PictureSelector
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelInfo = new Label();
            pictureBox = new PictureBox();
            buttonClear = new Button();
            comboBoxQuality = new ComboBox();
            comboBoxMaxDimension = new ComboBox();
            checkBoxDownsample = new CheckBox();
            checkBoxResize = new CheckBox();
            labelQuality = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            labelInfo.Location = new Point(84, 313);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(50, 15);
            labelInfo.TabIndex = 3;
            labelInfo.Text = "0x0, 0kB";
            // 
            // pictureBox
            // 
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.Location = new Point(3, 3);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(300, 300);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            // 
            // button1
            // 
            buttonClear.Location = new Point(3, 309);
            buttonClear.Name = "button1";
            buttonClear.Size = new Size(75, 23);
            buttonClear.TabIndex = 4;
            buttonClear.Text = "Clear";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            // 
            // comboBoxQuality
            // 
            comboBoxQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxQuality.FormattingEnabled = true;
            comboBoxQuality.Items.AddRange(new object[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" });
            comboBoxQuality.Location = new Point(251, 336);
            comboBoxQuality.Name = "comboBoxQuality";
            comboBoxQuality.Size = new Size(50, 23);
            comboBoxQuality.TabIndex = 7;
            comboBoxQuality.Visible = false;
            comboBoxQuality.SelectedIndexChanged += comboBoxQuality_SelectedIndexChanged;
            // 
            // comboBoxMaxDimension
            // 
            comboBoxMaxDimension.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMaxDimension.Enabled = false;
            comboBoxMaxDimension.FormattingEnabled = true;
            comboBoxMaxDimension.Items.AddRange(new object[] { "1000x1000", "1250x1250", "1500x1500", "2000x2000" });
            comboBoxMaxDimension.Location = new Point(73, 336);
            comboBoxMaxDimension.Name = "comboBoxMaxDimension";
            comboBoxMaxDimension.Size = new Size(118, 23);
            comboBoxMaxDimension.TabIndex = 8;
            comboBoxMaxDimension.Visible = false;
            comboBoxMaxDimension.SelectedIndexChanged += comboBoxMaxDimension_SelectedIndexChanged;
            // 
            // checkBoxDownsample
            // 
            checkBoxDownsample.AutoSize = true;
            checkBoxDownsample.Location = new Point(242, 311);
            checkBoxDownsample.Name = "checkBoxDownsample";
            checkBoxDownsample.Size = new Size(59, 19);
            checkBoxDownsample.TabIndex = 9;
            checkBoxDownsample.Text = "Shrink";
            checkBoxDownsample.UseVisualStyleBackColor = true;
            checkBoxDownsample.CheckedChanged += checkBoxResize_CheckedChanged;
            // 
            // checkBoxResize
            // 
            checkBoxResize.AutoSize = true;
            checkBoxResize.Location = new Point(10, 338);
            checkBoxResize.Name = "checkBoxResize";
            checkBoxResize.Size = new Size(61, 19);
            checkBoxResize.TabIndex = 10;
            checkBoxResize.Text = "Resize:";
            checkBoxResize.UseVisualStyleBackColor = true;
            checkBoxResize.Visible = false;
            checkBoxResize.CheckedChanged += checkBoxResize_CheckedChanged_1;
            // 
            // labelQuality
            // 
            labelQuality.AutoSize = true;
            labelQuality.Location = new Point(197, 339);
            labelQuality.Name = "labelQuality";
            labelQuality.Size = new Size(48, 15);
            labelQuality.TabIndex = 11;
            labelQuality.Text = "Quality:";
            labelQuality.Visible = false;
            // 
            // PictureSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelQuality);
            Controls.Add(checkBoxResize);
            Controls.Add(checkBoxDownsample);
            Controls.Add(comboBoxMaxDimension);
            Controls.Add(comboBoxQuality);
            Controls.Add(buttonClear);
            Controls.Add(labelInfo);
            Controls.Add(pictureBox);
            Name = "PictureSelector";
            Size = new Size(307, 365);
            Load += PictureSelector_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelInfo;
        private PictureBox pictureBox;
        private Button buttonClear;
        private ComboBox comboBoxQuality;
        private ComboBox comboBoxMaxDimension;
        private CheckBox checkBoxDownsample;
        private CheckBox checkBoxResize;
        private Label labelQuality;
    }
}
