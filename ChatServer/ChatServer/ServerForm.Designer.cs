namespace ChatServer
{
    partial class ServerForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.bStartServer = new System.Windows.Forms.Button();
            this.tbServerName = new System.Windows.Forms.TextBox();
            this.lServerName = new System.Windows.Forms.Label();
            this.bStopServer = new System.Windows.Forms.Button();
            this.lIP = new System.Windows.Forms.Label();
            this.lHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bStartServer
            // 
            this.bStartServer.Location = new System.Drawing.Point(12, 51);
            this.bStartServer.Name = "bStartServer";
            this.bStartServer.Size = new System.Drawing.Size(75, 23);
            this.bStartServer.TabIndex = 0;
            this.bStartServer.Text = "Старт";
            this.bStartServer.UseVisualStyleBackColor = true;
            this.bStartServer.Click += new System.EventHandler(this.bStartServer_Click);
            // 
            // tbServerName
            // 
            this.tbServerName.Location = new System.Drawing.Point(12, 25);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(100, 20);
            this.tbServerName.TabIndex = 1;
            this.tbServerName.Text = "2DKot";
            // 
            // lServerName
            // 
            this.lServerName.AutoSize = true;
            this.lServerName.Location = new System.Drawing.Point(9, 9);
            this.lServerName.Name = "lServerName";
            this.lServerName.Size = new System.Drawing.Size(74, 13);
            this.lServerName.TabIndex = 2;
            this.lServerName.Text = "Имя сервера";
            // 
            // bStopServer
            // 
            this.bStopServer.Enabled = false;
            this.bStopServer.Location = new System.Drawing.Point(94, 50);
            this.bStopServer.Name = "bStopServer";
            this.bStopServer.Size = new System.Drawing.Size(75, 23);
            this.bStopServer.TabIndex = 3;
            this.bStopServer.Text = "Стоп";
            this.bStopServer.UseVisualStyleBackColor = true;
            this.bStopServer.Click += new System.EventHandler(this.bStopServer_Click);
            // 
            // lIP
            // 
            this.lIP.AutoSize = true;
            this.lIP.Location = new System.Drawing.Point(124, 28);
            this.lIP.Name = "lIP";
            this.lIP.Size = new System.Drawing.Size(45, 13);
            this.lIP.TabIndex = 5;
            this.lIP.Text = "Ваш ip: ";
            // 
            // lHelp
            // 
            this.lHelp.AutoSize = true;
            this.lHelp.Location = new System.Drawing.Point(251, 0);
            this.lHelp.Name = "lHelp";
            this.lHelp.Size = new System.Drawing.Size(13, 13);
            this.lHelp.TabIndex = 6;
            this.lHelp.Tag = "";
            this.lHelp.Text = "?";
            this.lHelp.Click += new System.EventHandler(this.lHelp_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 80);
            this.Controls.Add(this.lHelp);
            this.Controls.Add(this.lIP);
            this.Controls.Add(this.bStopServer);
            this.Controls.Add(this.lServerName);
            this.Controls.Add(this.tbServerName);
            this.Controls.Add(this.bStartServer);
            this.Name = "ServerForm";
            this.Text = "ChatServer";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bStartServer;
        private System.Windows.Forms.TextBox tbServerName;
        private System.Windows.Forms.Label lServerName;
        private System.Windows.Forms.Button bStopServer;
        private System.Windows.Forms.Label lIP;
        private System.Windows.Forms.Label lHelp;
    }
}