namespace ChatClient
{
    partial class ClientForm
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
            this.IPAdressAndPortTextBox = new System.Windows.Forms.TextBox();
            this.ipAdressLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.RequestNickNameButton = new System.Windows.Forms.Button();
            this.NickNamesListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DebugCheckBox = new System.Windows.Forms.CheckBox();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.AddNewGetterButton = new System.Windows.Forms.Button();
            this.GettersFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.GettersPanelLabel = new System.Windows.Forms.Label();
            this.AuthorizationTabControl = new System.Windows.Forms.TabControl();
            this.CurrentNickNameTextBox = new System.Windows.Forms.TextBox();
            this.SignInTabPage = new System.Windows.Forms.TabPage();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.LoginLabel = new System.Windows.Forms.Label();
            this.LogInButton = new System.Windows.Forms.Button();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.LoginTextBox = new System.Windows.Forms.TextBox();
            this.SignUpTabPage = new System.Windows.Forms.TabPage();
            this.RegPasswordLabel = new System.Windows.Forms.Label();
            this.RegLoginLabel = new System.Windows.Forms.Label();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.RegPasswordTextBox = new System.Windows.Forms.TextBox();
            this.RegLoginTextBox = new System.Windows.Forms.TextBox();
            this.GuestAuthorizationTabPage = new System.Windows.Forms.TabPage();
            this.GuestLoginLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ServerNameLabel = new System.Windows.Forms.Label();
            this.ServerNameTextBox = new System.Windows.Forms.TextBox();
            this.AuthorizationTabControl.SuspendLayout();
            this.SignInTabPage.SuspendLayout();
            this.SignUpTabPage.SuspendLayout();
            this.GuestAuthorizationTabPage.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RTMainChatBox
            // 
            this.RTMainChatBox.Location = new System.Drawing.Point(12, 57);
            this.RTMainChatBox.Name = "RTMainChatBox";
            this.RTMainChatBox.ReadOnly = true;
            this.RTMainChatBox.Size = new System.Drawing.Size(411, 266);
            this.RTMainChatBox.TabIndex = 0;
            this.RTMainChatBox.Text = "";
            // 
            // TypingBox
            // 
            this.TypingBox.Location = new System.Drawing.Point(278, 386);
            this.TypingBox.Name = "TypingBox";
            this.TypingBox.Size = new System.Drawing.Size(285, 20);
            this.TypingBox.TabIndex = 1;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(569, 383);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 25);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Отправить";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // IPAdressAndPortTextBox
            // 
            this.IPAdressAndPortTextBox.Location = new System.Drawing.Point(97, 33);
            this.IPAdressAndPortTextBox.Name = "IPAdressAndPortTextBox";
            this.IPAdressAndPortTextBox.ReadOnly = true;
            this.IPAdressAndPortTextBox.Size = new System.Drawing.Size(123, 20);
            this.IPAdressAndPortTextBox.TabIndex = 3;
            // 
            // ipAdressLabel
            // 
            this.ipAdressLabel.AutoSize = true;
            this.ipAdressLabel.Location = new System.Drawing.Point(97, 17);
            this.ipAdressLabel.Name = "ipAdressLabel";
            this.ipAdressLabel.Size = new System.Drawing.Size(85, 13);
            this.ipAdressLabel.TabIndex = 5;
            this.ipAdressLabel.Text = "IP-адрес и порт";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(12, 4);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(79, 23);
            this.ConnectButton.TabIndex = 7;
            this.ConnectButton.Text = "Подключить";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // RequestNickNameButton
            // 
            this.RequestNickNameButton.Location = new System.Drawing.Point(126, 36);
            this.RequestNickNameButton.Name = "RequestNickNameButton";
            this.RequestNickNameButton.Size = new System.Drawing.Size(75, 23);
            this.RequestNickNameButton.TabIndex = 10;
            this.RequestNickNameButton.Text = "Запрос";
            this.RequestNickNameButton.UseVisualStyleBackColor = true;
            this.RequestNickNameButton.Click += new System.EventHandler(this.RequestNickNameButton_Click);
            // 
            // NickNamesListBox
            // 
            this.NickNamesListBox.FormattingEnabled = true;
            this.NickNamesListBox.Location = new System.Drawing.Point(429, 175);
            this.NickNamesListBox.Name = "NickNamesListBox";
            this.NickNamesListBox.Size = new System.Drawing.Size(215, 173);
            this.NickNamesListBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(529, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Пользователи в сети";
            // 
            // DebugCheckBox
            // 
            this.DebugCheckBox.AutoSize = true;
            this.DebugCheckBox.Location = new System.Drawing.Point(569, 9);
            this.DebugCheckBox.Name = "DebugCheckBox";
            this.DebugCheckBox.Size = new System.Drawing.Size(88, 17);
            this.DebugCheckBox.TabIndex = 13;
            this.DebugCheckBox.Text = "Debug Mode";
            this.DebugCheckBox.UseVisualStyleBackColor = true;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Enabled = false;
            this.DisconnectButton.Location = new System.Drawing.Point(12, 33);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(79, 23);
            this.DisconnectButton.TabIndex = 14;
            this.DisconnectButton.Text = "Отключить";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // AddNewGetterButton
            // 
            this.AddNewGetterButton.Location = new System.Drawing.Point(569, 354);
            this.AddNewGetterButton.Name = "AddNewGetterButton";
            this.AddNewGetterButton.Size = new System.Drawing.Size(75, 23);
            this.AddNewGetterButton.TabIndex = 17;
            this.AddNewGetterButton.Text = "Добавить";
            this.AddNewGetterButton.UseVisualStyleBackColor = true;
            this.AddNewGetterButton.Click += new System.EventHandler(this.AddNewGetterButton_Click);
            // 
            // GettersFlowLayoutPanel
            // 
            this.GettersFlowLayoutPanel.AutoScroll = true;
            this.GettersFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GettersFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.GettersFlowLayoutPanel.Location = new System.Drawing.Point(12, 369);
            this.GettersFlowLayoutPanel.Name = "GettersFlowLayoutPanel";
            this.GettersFlowLayoutPanel.Size = new System.Drawing.Size(260, 49);
            this.GettersFlowLayoutPanel.TabIndex = 18;
            // 
            // GettersPanelLabel
            // 
            this.GettersPanelLabel.AutoSize = true;
            this.GettersPanelLabel.Location = new System.Drawing.Point(12, 326);
            this.GettersPanelLabel.Name = "GettersPanelLabel";
            this.GettersPanelLabel.Size = new System.Drawing.Size(111, 13);
            this.GettersPanelLabel.TabIndex = 19;
            this.GettersPanelLabel.Text = "Панель получателей";
            // 
            // AuthorizationTabControl
            // 
            this.AuthorizationTabControl.Controls.Add(this.SignInTabPage);
            this.AuthorizationTabControl.Controls.Add(this.SignUpTabPage);
            this.AuthorizationTabControl.Controls.Add(this.GuestAuthorizationTabPage);
            this.AuthorizationTabControl.Location = new System.Drawing.Point(3, 3);
            this.AuthorizationTabControl.Name = "AuthorizationTabControl";
            this.AuthorizationTabControl.SelectedIndex = 0;
            this.AuthorizationTabControl.Size = new System.Drawing.Size(215, 111);
            this.AuthorizationTabControl.TabIndex = 0;
            // 
            // CurrentNickNameTextBox
            // 
            this.CurrentNickNameTextBox.Location = new System.Drawing.Point(6, 20);
            this.CurrentNickNameTextBox.Name = "CurrentNickNameTextBox";
            this.CurrentNickNameTextBox.ReadOnly = true;
            this.CurrentNickNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.CurrentNickNameTextBox.TabIndex = 24;
            // 
            // SignInTabPage
            // 
            this.SignInTabPage.Controls.Add(this.PasswordLabel);
            this.SignInTabPage.Controls.Add(this.LoginLabel);
            this.SignInTabPage.Controls.Add(this.LogInButton);
            this.SignInTabPage.Controls.Add(this.PasswordTextBox);
            this.SignInTabPage.Controls.Add(this.LoginTextBox);
            this.SignInTabPage.Location = new System.Drawing.Point(4, 22);
            this.SignInTabPage.Name = "SignInTabPage";
            this.SignInTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SignInTabPage.Size = new System.Drawing.Size(207, 85);
            this.SignInTabPage.TabIndex = 1;
            this.SignInTabPage.Text = "Войти";
            this.SignInTabPage.UseVisualStyleBackColor = true;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(7, 41);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(45, 13);
            this.PasswordLabel.TabIndex = 4;
            this.PasswordLabel.Text = "Пароль";
            // 
            // LoginLabel
            // 
            this.LoginLabel.AutoSize = true;
            this.LoginLabel.Location = new System.Drawing.Point(7, 4);
            this.LoginLabel.Name = "LoginLabel";
            this.LoginLabel.Size = new System.Drawing.Size(38, 13);
            this.LoginLabel.TabIndex = 3;
            this.LoginLabel.Text = "Логин";
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(126, 36);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(75, 23);
            this.LogInButton.TabIndex = 2;
            this.LogInButton.Text = "Войти";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(6, 59);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.PasswordTextBox.TabIndex = 1;
            // 
            // LoginTextBox
            // 
            this.LoginTextBox.Location = new System.Drawing.Point(6, 20);
            this.LoginTextBox.Name = "LoginTextBox";
            this.LoginTextBox.Size = new System.Drawing.Size(100, 20);
            this.LoginTextBox.TabIndex = 0;
            // 
            // SignUpTabPage
            // 
            this.SignUpTabPage.Controls.Add(this.RegPasswordLabel);
            this.SignUpTabPage.Controls.Add(this.RegLoginLabel);
            this.SignUpTabPage.Controls.Add(this.RegisterButton);
            this.SignUpTabPage.Controls.Add(this.RegPasswordTextBox);
            this.SignUpTabPage.Controls.Add(this.RegLoginTextBox);
            this.SignUpTabPage.Location = new System.Drawing.Point(4, 22);
            this.SignUpTabPage.Name = "SignUpTabPage";
            this.SignUpTabPage.Size = new System.Drawing.Size(207, 85);
            this.SignUpTabPage.TabIndex = 2;
            this.SignUpTabPage.Text = "Регистрация";
            this.SignUpTabPage.UseVisualStyleBackColor = true;
            // 
            // RegPasswordLabel
            // 
            this.RegPasswordLabel.AutoSize = true;
            this.RegPasswordLabel.Location = new System.Drawing.Point(7, 41);
            this.RegPasswordLabel.Name = "RegPasswordLabel";
            this.RegPasswordLabel.Size = new System.Drawing.Size(45, 13);
            this.RegPasswordLabel.TabIndex = 9;
            this.RegPasswordLabel.Text = "Пароль";
            // 
            // RegLoginLabel
            // 
            this.RegLoginLabel.AutoSize = true;
            this.RegLoginLabel.Location = new System.Drawing.Point(7, 4);
            this.RegLoginLabel.Name = "RegLoginLabel";
            this.RegLoginLabel.Size = new System.Drawing.Size(125, 13);
            this.RegLoginLabel.TabIndex = 8;
            this.RegLoginLabel.Text = "Регистриуремый логин";
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(126, 36);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(75, 23);
            this.RegisterButton.TabIndex = 7;
            this.RegisterButton.Text = "Запрос";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // RegPasswordTextBox
            // 
            this.RegPasswordTextBox.Location = new System.Drawing.Point(6, 59);
            this.RegPasswordTextBox.Name = "RegPasswordTextBox";
            this.RegPasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.RegPasswordTextBox.TabIndex = 6;
            // 
            // RegLoginTextBox
            // 
            this.RegLoginTextBox.Location = new System.Drawing.Point(6, 20);
            this.RegLoginTextBox.Name = "RegLoginTextBox";
            this.RegLoginTextBox.Size = new System.Drawing.Size(100, 20);
            this.RegLoginTextBox.TabIndex = 5;
            // 
            // GuestAuthorizationTabPage
            // 
            this.GuestAuthorizationTabPage.Controls.Add(this.CurrentNickNameTextBox);
            this.GuestAuthorizationTabPage.Controls.Add(this.GuestLoginLabel);
            this.GuestAuthorizationTabPage.Controls.Add(this.RequestNickNameButton);
            this.GuestAuthorizationTabPage.Location = new System.Drawing.Point(4, 22);
            this.GuestAuthorizationTabPage.Name = "GuestAuthorizationTabPage";
            this.GuestAuthorizationTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GuestAuthorizationTabPage.Size = new System.Drawing.Size(207, 85);
            this.GuestAuthorizationTabPage.TabIndex = 0;
            this.GuestAuthorizationTabPage.Text = "Гость";
            this.GuestAuthorizationTabPage.UseVisualStyleBackColor = true;
            // 
            // GuestLoginLabel
            // 
            this.GuestLoginLabel.AutoSize = true;
            this.GuestLoginLabel.Location = new System.Drawing.Point(7, 4);
            this.GuestLoginLabel.Name = "GuestLoginLabel";
            this.GuestLoginLabel.Size = new System.Drawing.Size(116, 13);
            this.GuestLoginLabel.TabIndex = 11;
            this.GuestLoginLabel.Text = "Временный ник-нейм";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.AuthorizationTabControl);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(429, 28);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(227, 112);
            this.flowLayoutPanel1.TabIndex = 23;
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Location = new System.Drawing.Point(223, 17);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(74, 13);
            this.ServerNameLabel.TabIndex = 25;
            this.ServerNameLabel.Text = "Имя сервера";
            // 
            // ServerNameTextBox
            // 
            this.ServerNameTextBox.Location = new System.Drawing.Point(226, 33);
            this.ServerNameTextBox.Name = "ServerNameTextBox";
            this.ServerNameTextBox.ReadOnly = true;
            this.ServerNameTextBox.Size = new System.Drawing.Size(119, 20);
            this.ServerNameTextBox.TabIndex = 26;
            // 
            // ClientForm
            // 
            this.AcceptButton = this.SendButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 441);
            this.Controls.Add(this.ServerNameTextBox);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.GettersPanelLabel);
            this.Controls.Add(this.GettersFlowLayoutPanel);
            this.Controls.Add(this.AddNewGetterButton);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.DebugCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NickNamesListBox);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ipAdressLabel);
            this.Controls.Add(this.IPAdressAndPortTextBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.TypingBox);
            this.Controls.Add(this.RTMainChatBox);
            this.Name = "ClientForm";
            this.Text = "Chat-Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientWindow_FormClosing);
            this.AuthorizationTabControl.ResumeLayout(false);
            this.SignInTabPage.ResumeLayout(false);
            this.SignInTabPage.PerformLayout();
            this.SignUpTabPage.ResumeLayout(false);
            this.SignUpTabPage.PerformLayout();
            this.GuestAuthorizationTabPage.ResumeLayout(false);
            this.GuestAuthorizationTabPage.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox RTMainChatBox;
        private System.Windows.Forms.TextBox TypingBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox IPAdressAndPortTextBox;
        private System.Windows.Forms.Label ipAdressLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button RequestNickNameButton;
        private System.Windows.Forms.ListBox NickNamesListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DebugCheckBox;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.Button AddNewGetterButton;
        private System.Windows.Forms.FlowLayoutPanel GettersFlowLayoutPanel;
        private System.Windows.Forms.Label GettersPanelLabel;
        private System.Windows.Forms.TabControl AuthorizationTabControl;
        private System.Windows.Forms.TabPage SignInTabPage;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label LoginLabel;
        private System.Windows.Forms.Button LogInButton;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox LoginTextBox;
        private System.Windows.Forms.TabPage SignUpTabPage;
        private System.Windows.Forms.Label RegPasswordLabel;
        private System.Windows.Forms.Label RegLoginLabel;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.TextBox RegPasswordTextBox;
        private System.Windows.Forms.TextBox RegLoginTextBox;
        private System.Windows.Forms.TabPage GuestAuthorizationTabPage;
        private System.Windows.Forms.Label GuestLoginLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox CurrentNickNameTextBox;
        private System.Windows.Forms.Label ServerNameLabel;
        private System.Windows.Forms.TextBox ServerNameTextBox;
    }
}

