namespace ID3Tagger
{
    partial class FormPicture
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonOk = new Button();
            buttonCancel = new Button();
            pictureSelector1 = new PictureSelector();
            SuspendLayout();
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(162, 357);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(75, 23);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(243, 357);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // pictureSelector1
            // 
            pictureSelector1.Location = new Point(12, 12);
            pictureSelector1.Name = "pictureSelector1";
            pictureSelector1.Size = new Size(307, 339);
            pictureSelector1.TabIndex = 4;
            // 
            // FormPicture
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(330, 392);
            Controls.Add(pictureSelector1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormPicture";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Album cover";
            ResumeLayout(false);
        }

        #endregion
        private Button buttonOk;
        private Button buttonCancel;
        private PictureSelector pictureSelector1;
    }
}