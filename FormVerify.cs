
namespace ID3Tagger
{
    public partial class FormVerify : Form
    {
        public FormVerify()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToLower() == "yes")
                DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) buttonOk_Click(null, null);
        }
    }
}
