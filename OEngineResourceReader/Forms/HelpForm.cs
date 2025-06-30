using OEngineResourceReader.Utils;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace OEngineResourceReader.Forms
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
           
            string readmePath =  CultureInfo.CurrentCulture.Name switch
            {
                "uk-UA" => "Readme_ua.txt",
                "ru-RU" => "Readme_ua.txt",
                _ => "Readme.txt"
            };

            if (File.Exists(readmePath))
            {
                string helpTextContent = File.ReadAllText(readmePath, Encoding.UTF8);
                helpTextBox.Text = helpTextContent.Replace("%version%", Helpers.GetApplicationVersion());
            }
        }

        private void helpTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.LinkText) {
                    UseShellExecute = true 
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
