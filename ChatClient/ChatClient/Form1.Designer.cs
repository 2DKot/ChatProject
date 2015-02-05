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
            this.SuspendLayout();
            // 
            // RTMainChatBox
            // 
            this.RTMainChatBox.Location = new System.Drawing.Point(12, 53);
            this.RTMainChatBox.Name = "RTMainChatBox";
            this.RTMainChatBox.Size = new System.Drawing.Size(285, 256);
            this.RTMainChatBox.TabIndex = 0;
            this.RTMainChatBox.Text = "";
            // 
            // TypingBox
            // 
            this.TypingBox.Location = new System.Drawing.Point(12, 315);
            this.TypingBox.Name = "TypingBox";
            this.TypingBox.Size = new System.Drawing.Size(285, 20);
            this.TypingBox.TabIndex = 1;
            this.TypingBox.TextChanged += new System.EventHandler(this.TypingBox_TextChanged);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(273, 341);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 25);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Отправить";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 378);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.TypingBox);
            this.Controls.Add(this.RTMainChatBox);
            this.Name = "ClientWindow";
            this.Text = "Chat-Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox RTMainChatBox;
        private System.Windows.Forms.TextBox TypingBox;
        private System.Windows.Forms.Button SendButton;
    }
}

