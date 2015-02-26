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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.bStartServer = new System.Windows.Forms.Button();
            this.tbServerName = new System.Windows.Forms.TextBox();
            this.lServerName = new System.Windows.Forms.Label();
            this.bStopServer = new System.Windows.Forms.Button();
            this.lIP = new System.Windows.Forms.Label();
            this.lHelp = new System.Windows.Forms.Label();
            this.lbUsers = new System.Windows.Forms.ListBox();
            this.bKick = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.bSendToAll = new System.Windows.Forms.Button();
            this.gbSendMessage = new System.Windows.Forms.GroupBox();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            this.bSendToCurrent = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.gbUsers = new System.Windows.Forms.GroupBox();
            this.bClearLog = new System.Windows.Forms.Button();
            this.bDeleteLogFile = new System.Windows.Forms.Button();
            this.lState = new System.Windows.Forms.Label();
            this.gbSendMessage.SuspendLayout();
            this.gbUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // bStartServer
            // 
            this.bStartServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bStartServer.Location = new System.Drawing.Point(12, 51);
            this.bStartServer.Name = "bStartServer";
            this.bStartServer.Size = new System.Drawing.Size(75, 23);
            this.bStartServer.TabIndex = 1;
            this.bStartServer.Text = "Старт";
            this.bStartServer.UseVisualStyleBackColor = false;
            this.bStartServer.Click += new System.EventHandler(this.bStartServer_Click);
            // 
            // tbServerName
            // 
            this.tbServerName.Location = new System.Drawing.Point(12, 25);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(100, 20);
            this.tbServerName.TabIndex = 0;
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
            this.bStopServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bStopServer.Location = new System.Drawing.Point(12, 51);
            this.bStopServer.Name = "bStopServer";
            this.bStopServer.Size = new System.Drawing.Size(75, 23);
            this.bStopServer.TabIndex = 3;
            this.bStopServer.Text = "Стоп";
            this.bStopServer.UseVisualStyleBackColor = false;
            this.bStopServer.Visible = false;
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
            this.lHelp.Location = new System.Drawing.Point(794, 0);
            this.lHelp.Name = "lHelp";
            this.lHelp.Size = new System.Drawing.Size(13, 13);
            this.lHelp.TabIndex = 6;
            this.lHelp.Tag = "";
            this.lHelp.Text = "?";
            this.lHelp.Click += new System.EventHandler(this.lHelp_Click);
            // 
            // lbUsers
            // 
            this.lbUsers.FormattingEnabled = true;
            this.lbUsers.Location = new System.Drawing.Point(6, 17);
            this.lbUsers.Name = "lbUsers";
            this.lbUsers.Size = new System.Drawing.Size(276, 95);
            this.lbUsers.TabIndex = 7;
            // 
            // bKick
            // 
            this.bKick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bKick.Location = new System.Drawing.Point(6, 118);
            this.bKick.Name = "bKick";
            this.bKick.Size = new System.Drawing.Size(75, 23);
            this.bKick.TabIndex = 8;
            this.bKick.Text = "Выпнуть!";
            this.bKick.UseVisualStyleBackColor = true;
            this.bKick.Click += new System.EventHandler(this.bKick_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(6, 19);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(231, 20);
            this.tbMessage.TabIndex = 9;
            // 
            // bSendToAll
            // 
            this.bSendToAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSendToAll.Location = new System.Drawing.Point(6, 45);
            this.bSendToAll.Name = "bSendToAll";
            this.bSendToAll.Size = new System.Drawing.Size(112, 23);
            this.bSendToAll.TabIndex = 10;
            this.bSendToAll.Text = "Всем";
            this.bSendToAll.UseVisualStyleBackColor = true;
            this.bSendToAll.Click += new System.EventHandler(this.bSendToAll_Click);
            // 
            // gbSendMessage
            // 
            this.gbSendMessage.Controls.Add(this.cbDebug);
            this.gbSendMessage.Controls.Add(this.bSendToCurrent);
            this.gbSendMessage.Controls.Add(this.tbMessage);
            this.gbSendMessage.Controls.Add(this.bSendToAll);
            this.gbSendMessage.Location = new System.Drawing.Point(6, 147);
            this.gbSendMessage.Name = "gbSendMessage";
            this.gbSendMessage.Size = new System.Drawing.Size(276, 103);
            this.gbSendMessage.TabIndex = 11;
            this.gbSendMessage.TabStop = false;
            this.gbSendMessage.Text = "Послать сообщение";
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.Location = new System.Drawing.Point(6, 80);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(88, 17);
            this.cbDebug.TabIndex = 12;
            this.cbDebug.Text = "Debug Mode";
            this.cbDebug.UseVisualStyleBackColor = true;
            // 
            // bSendToCurrent
            // 
            this.bSendToCurrent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSendToCurrent.Location = new System.Drawing.Point(125, 45);
            this.bSendToCurrent.Name = "bSendToCurrent";
            this.bSendToCurrent.Size = new System.Drawing.Size(112, 23);
            this.bSendToCurrent.TabIndex = 11;
            this.bSendToCurrent.Text = "Выделенному";
            this.bSendToCurrent.UseVisualStyleBackColor = true;
            this.bSendToCurrent.Click += new System.EventHandler(this.bSendToCurrent_Click);
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tbLog.Location = new System.Drawing.Point(12, 81);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(487, 194);
            this.tbLog.TabIndex = 12;
            this.tbLog.TabStop = false;
            // 
            // gbUsers
            // 
            this.gbUsers.Controls.Add(this.lbUsers);
            this.gbUsers.Controls.Add(this.bKick);
            this.gbUsers.Controls.Add(this.gbSendMessage);
            this.gbUsers.Location = new System.Drawing.Point(505, 9);
            this.gbUsers.Name = "gbUsers";
            this.gbUsers.Size = new System.Drawing.Size(291, 266);
            this.gbUsers.TabIndex = 13;
            this.gbUsers.TabStop = false;
            this.gbUsers.Text = "Юзеры";
            // 
            // bClearLog
            // 
            this.bClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClearLog.Location = new System.Drawing.Point(242, 51);
            this.bClearLog.Name = "bClearLog";
            this.bClearLog.Size = new System.Drawing.Size(99, 23);
            this.bClearLog.TabIndex = 14;
            this.bClearLog.Text = "Очистить лог";
            this.bClearLog.UseVisualStyleBackColor = true;
            this.bClearLog.Click += new System.EventHandler(this.bClearLog_Click);
            // 
            // bDeleteLogFile
            // 
            this.bDeleteLogFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bDeleteLogFile.Location = new System.Drawing.Point(347, 51);
            this.bDeleteLogFile.Name = "bDeleteLogFile";
            this.bDeleteLogFile.Size = new System.Drawing.Size(126, 23);
            this.bDeleteLogFile.TabIndex = 15;
            this.bDeleteLogFile.Text = "Удалить файл лога";
            this.bDeleteLogFile.UseVisualStyleBackColor = true;
            this.bDeleteLogFile.Click += new System.EventHandler(this.bDeleteLogFile_Click);
            // 
            // lState
            // 
            this.lState.AutoSize = true;
            this.lState.Location = new System.Drawing.Point(93, 56);
            this.lState.Name = "lState";
            this.lState.Size = new System.Drawing.Size(35, 13);
            this.lState.TabIndex = 16;
            this.lState.Text = "offline";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(808, 287);
            this.Controls.Add(this.lState);
            this.Controls.Add(this.bDeleteLogFile);
            this.Controls.Add(this.bClearLog);
            this.Controls.Add(this.gbUsers);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.lHelp);
            this.Controls.Add(this.lIP);
            this.Controls.Add(this.bStopServer);
            this.Controls.Add(this.lServerName);
            this.Controls.Add(this.tbServerName);
            this.Controls.Add(this.bStartServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerForm";
            this.Text = "ChatServer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerForm_FormClosed);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.gbSendMessage.ResumeLayout(false);
            this.gbSendMessage.PerformLayout();
            this.gbUsers.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox lbUsers;
        private System.Windows.Forms.Button bKick;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button bSendToAll;
        private System.Windows.Forms.GroupBox gbSendMessage;
        private System.Windows.Forms.Button bSendToCurrent;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.GroupBox gbUsers;
        private System.Windows.Forms.Button bClearLog;
        private System.Windows.Forms.CheckBox cbDebug;
        private System.Windows.Forms.Button bDeleteLogFile;
        private System.Windows.Forms.Label lState;
    }
}