namespace OEngineResourceReader.Forms
{
    partial class HelpForm
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
            helpTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // helpTextBox
            // 
            helpTextBox.BorderStyle = BorderStyle.FixedSingle;
            helpTextBox.Dock = DockStyle.Fill;
            helpTextBox.Location = new Point(0, 0);
            helpTextBox.Name = "helpTextBox";
            helpTextBox.ReadOnly = true;
            helpTextBox.Size = new Size(800, 450);
            helpTextBox.TabIndex = 0;
            helpTextBox.Text = "";
            helpTextBox.LinkClicked += helpTextBox_LinkClicked;
            // 
            // HelpForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(helpTextBox);
            Name = "HelpForm";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "Help";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox helpTextBox;
    }
}