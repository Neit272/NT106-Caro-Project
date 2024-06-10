namespace Game_Caro
{
    partial class ClientInterface
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
            this.queueBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // queueBtn
            // 
            this.queueBtn.Location = new System.Drawing.Point(137, 71);
            this.queueBtn.Name = "queueBtn";
            this.queueBtn.Size = new System.Drawing.Size(209, 119);
            this.queueBtn.TabIndex = 0;
            this.queueBtn.Text = "Tạo trận";
            this.queueBtn.UseVisualStyleBackColor = true;
            // 
            // ClientInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 260);
            this.Controls.Add(this.queueBtn);
            this.Name = "ClientInterface";
            this.Text = "Client Interface";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button queueBtn;
    }
}