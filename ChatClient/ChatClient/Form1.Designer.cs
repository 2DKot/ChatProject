namespace ChatClient
{
    partial class ClientWindow
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
            this.RTMainChatBox = new System.Windows.Forms.RichTextBox();
            this.TypingBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.IPAdressTextBox = new System.Windows.Forms.TextBox();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.ipAdressLabel = new System.Windows.Forms.Label();
            this.PortLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.NickNameLabel = new System.Windows.Forms.Label();
            this.NickNameTextBox = new System.Windows.Forms.TextBox();
            this.RequestNickNameButton = new System.Windows.Forms.Button();
            this.NickNamesListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DebugCheckBox = new System.Windows.Forms.CheckBox();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RTMainChatBox
            // 
            this.RTMainChatBox.Location = new System.Drawing.Point(12, 53);
            this.RTMainChatBox.Name = "RTMainChatBox";
            this.RTMainChatBox.ReadOnly = true;
            this.RTMainChatBox.Size = new System.Drawing.Size(285, 291);
            this.RTMainChatBox.TabIndex = 0;
            this.RTMainChatBox.Text = "";
            // 
            // TypingBox
            // 
            this.TypingBox.Location = new System.Drawing.Point(12, 350);
            this.TypingBox.Name = "TypingBox";
            this.TypingBox.Size = new System.Drawing.Size(285, 20);
            this.TypingBox.TabIndex = 1;
            this.TypingBox.TextChanged += new System.EventHandler(this.TypingBox_TextChanged);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(303, 350);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 25);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Отправить";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // IPAdressTextBox
            // 
            this.IPAdressTextBox.Location = new System.Drawing.Point(91, 28);
            this.IPAdressTextBox.Name = "IPAdressTextBox";
            this.IPAdressTextBox.Size = new System.Drawing.Size(100, 20);
            this.IPAdressTextBox.TabIndex = 3;
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(197, 28);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(100, 20);
            this.PortTextBox.TabIndex = 4;
            // 
            // ipAdressLabel
            // 
            this.ipAdressLabel.AutoSize = true;
            this.ipAdressLabel.Location = new System.Drawing.Point(91, 8);
            this.ipAdressLabel.Name = "ipAdressLabel";
            this.ipAdressLabel.Size = new System.Drawing.Size(50, 13);
            this.ipAdressLabel.TabIndex = 5;
            this.ipAdressLabel.Text = "IP-адрес";
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(205, 8);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(32, 13);
            this.PortLabel.TabIndex = 6;
            this.PortLabel.Text = "Порт";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(6, 3);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(79, 23);
            this.ConnectButton.TabIndex = 7;
            this.ConnectButton.Text = "Подключить";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // NickNameLabel
            // 
            this.NickNameLabel.AutoSize = true;
            this.NickNameLabel.Location = new System.Drawing.Point(300, 56);
            this.NickNameLabel.Name = "NickNameLabel";
            this.NickNameLabel.Size = new System.Drawing.Size(56, 13);
            this.NickNameLabel.TabIndex = 8;
            this.NickNameLabel.Text = "Ник-нейм";
            // 
            // NickNameTextBox
            // 
            this.NickNameTextBox.Location = new System.Drawing.Point(303, 72);
            this.NickNameTextBox.Name = "NickNameTextBox";
            this.NickNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.NickNameTextBox.TabIndex = 9;
            // 
            // RequestNickNameButton
            // 
            this.RequestNickNameButton.Location = new System.Drawing.Point(328, 98);
            this.RequestNickNameButton.Name = "RequestNickNameButton";
            this.RequestNickNameButton.Size = new System.Drawing.Size(75, 23);
            this.RequestNickNameButton.TabIndex = 10;
            this.RequestNickNameButton.Text = "Сменить";
            this.RequestNickNameButton.UseVisualStyleBackColor = true;
            this.RequestNickNameButton.Click += new System.EventHandler(this.RequestNickNameButton_Click);
            // 
            // NickNamesListBox
            // 
            this.NickNamesListBox.FormattingEnabled = true;
            this.NickNamesListBox.Location = new System.Drawing.Point(303, 145);
            this.NickNamesListBox.Name = "NickNamesListBox";
            this.NickNamesListBox.Size = new System.Drawing.Size(115, 199);
            this.NickNamesListBox.TabIndex = 11;
            this.NickNamesListBox.SelectedIndexChanged += new System.EventHandler(this.NickNamesListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Пользователи в сети";
            // 
            // DebugCheckBox
            // 
            this.DebugCheckBox.AutoSize = true;
            this.DebugCheckBox.Location = new System.Drawing.Point(322, 13);
            this.DebugCheckBox.Name = "DebugCheckBox";
            this.DebugCheckBox.Size = new System.Drawing.Size(88, 17);
            this.DebugCheckBox.TabIndex = 13;
            this.DebugCheckBox.Text = "Debug Mode";
            this.DebugCheckBox.UseVisualStyleBackColor = true;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Enabled = false;
            this.DisconnectButton.Location = new System.Drawing.Point(6, 28);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(79, 23);
            this.DisconnectButton.TabIndex = 14;
            this.DisconnectButton.Text = "Отключить";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // ClientWindow
            // 
            this.AcceptButton = this.SendButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 387);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.DebugCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NickNamesListBox);
            this.Controls.Add(this.RequestNickNameButton);
            this.Controls.Add(this.NickNameTextBox);
            this.Controls.Add(this.NickNameLabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.ipAdressLabel);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.IPAdressTextBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.TypingBox);
            this.Controls.Add(this.RTMainChatBox);
            this.Name = "ClientWindow";
            this.Text = "Chat-Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox RTMainChatBox;
        private System.Windows.Forms.TextBox TypingBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox IPAdressTextBox;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label ipAdressLabel;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label NickNameLabel;
        private System.Windows.Forms.TextBox NickNameTextBox;
        private System.Windows.Forms.Button RequestNickNameButton;
        private System.Windows.Forms.ListBox NickNamesListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DebugCheckBox;
        private System.Windows.Forms.Button DisconnectButton;
    }
}

