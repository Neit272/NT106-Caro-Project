namespace Game_Caro
{
    partial class Server
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
            this.serverMonitor = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // serverMonitor
            // 
            this.serverMonitor.Location = new System.Drawing.Point(12, 12);
            this.serverMonitor.Name = "serverMonitor";
            this.serverMonitor.Size = new System.Drawing.Size(776, 506);
            this.serverMonitor.TabIndex = 0;
            this.serverMonitor.Text = "";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 530);
            this.Controls.Add(this.serverMonitor);
            this.Name = "Server";
            this.Text = "Server Monitor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox serverMonitor;
    }
}