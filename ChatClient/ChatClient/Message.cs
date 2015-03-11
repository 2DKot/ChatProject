using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace ChatClient
{
    class Message
    {
        string textMessage;
        Color textColor;

        public Message(string textMessage)
        {
            this.textMessage = textMessage;
            this.textColor = Color.Black;
        }
        public Message(string textMessage, Color color)
            : this(textMessage)
        {
            this.textColor = color;
        }
        public string TextMessage
        {
            get
            {
                return textMessage;
            }
        }
        public Color TextColor
        {
            get
            {
                return textColor;
            }
        }
    }
}
