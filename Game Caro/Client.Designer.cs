namespace Game_Caro
{
    partial class Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.connectionBtn = new System.Windows.Forms.Button();
            this.queueBtn = new System.Windows.Forms.Button();
            this.usernameTb = new System.Windows.Forms.TextBox();
            this.usernameLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connectionBtn
            // 
            this.connectionBtn.BackColor = System.Drawing.Color.DeepPink;
            this.connectionBtn.Enabled = false;
            this.connectionBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionBtn.ForeColor = System.Drawing.Color.White;
            this.connectionBtn.Location = new System.Drawing.Point(163, 69);
            this.connectionBtn.Name = "connectionBtn";
            this.connectionBtn.Size = new System.Drawing.Size(252, 97);
            this.connectionBtn.TabIndex = 0;
            this.connectionBtn.Text = "Connect";
            this.connectionBtn.UseVisualStyleBackColor = false;
            this.connectionBtn.Click += new System.EventHandler(this.connectionBtn_Click);
            // 
            // queueBtn
            // 
            this.queueBtn.BackColor = System.Drawing.Color.DeepPink;
            this.queueBtn.Enabled = false;
            this.queueBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.queueBtn.ForeColor = System.Drawing.Color.White;
            this.queueBtn.Location = new System.Drawing.Point(163, 193);
            this.queueBtn.Name = "queueBtn";
            this.queueBtn.Size = new System.Drawing.Size(252, 97);
            this.queueBtn.TabIndex = 1;
            this.queueBtn.Text = "Queue";
            this.queueBtn.UseVisualStyleBackColor = false;
            this.queueBtn.Click += new System.EventHandler(this.queueBtn_Click);
            // 
            // usernameTb
            // 
            this.usernameTb.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameTb.Location = new System.Drawing.Point(270, 20);
            this.usernameTb.Name = "usernameTb";
            this.usernameTb.Size = new System.Drawing.Size(145, 30);
            this.usernameTb.TabIndex = 2;
            this.usernameTb.TextChanged += new System.EventHandler(this.usernameTb_TextChanged);
            // 
            // usernameLbl
            // 
            this.usernameLbl.AutoSize = true;
            this.usernameLbl.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameLbl.Location = new System.Drawing.Point(159, 27);
            this.usernameLbl.Name = "usernameLbl";
            this.usernameLbl.Size = new System.Drawing.Size(105, 23);
            this.usernameLbl.TabIndex = 3;
            this.usernameLbl.Text = "Username:";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(588, 311);
            this.Controls.Add(this.usernameLbl);
            this.Controls.Add(this.usernameTb);
            this.Controls.Add(this.queueBtn);
            this.Controls.Add(this.connectionBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Client";
            this.Text = "Client Interface";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectionBtn;
        private System.Windows.Forms.Button queueBtn;
        private System.Windows.Forms.TextBox usernameTb;
        private System.Windows.Forms.Label usernameLbl;
    }
}