namespace Game_Caro
{
    partial class Dashboard
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
            this.btnCreateServer = new System.Windows.Forms.Button();
            this.btnConnectAsClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateServer
            // 
            this.btnCreateServer.Location = new System.Drawing.Point(92, 94);
            this.btnCreateServer.Name = "btnCreateServer";
            this.btnCreateServer.Size = new System.Drawing.Size(163, 58);
            this.btnCreateServer.TabIndex = 0;
            this.btnCreateServer.Text = "Server";
            this.btnCreateServer.UseVisualStyleBackColor = true;
            this.btnCreateServer.Click += new System.EventHandler(this.btnCreateServer_Click);
            // 
            // btnConnectAsClient
            // 
            this.btnConnectAsClient.Location = new System.Drawing.Point(278, 94);
            this.btnConnectAsClient.Name = "btnConnectAsClient";
            this.btnConnectAsClient.Size = new System.Drawing.Size(163, 58);
            this.btnConnectAsClient.TabIndex = 1;
            this.btnConnectAsClient.Text = "Client";
            this.btnConnectAsClient.UseVisualStyleBackColor = true;
            this.btnConnectAsClient.Click += new System.EventHandler(this.btnConnectAsClient_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 246);
            this.Controls.Add(this.btnConnectAsClient);
            this.Controls.Add(this.btnCreateServer);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateServer;
        private System.Windows.Forms.Button btnConnectAsClient;
    }
}