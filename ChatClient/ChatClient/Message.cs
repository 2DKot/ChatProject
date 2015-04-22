﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace ChatClient
{
    class Message
    {
        string rawMessage;
        string textMessage;
        Color textColor;

        public Message(string rawMessage, string textMessage)
        {
            this.textMessage = textMessage;
            this.textColor = Color.Black;
            this.rawMessage = rawMessage;
        }
        public Message(string rawMessage, string textMessage, Color color)
            : this(rawMessage, textMessage)
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
        public string RawMessage
        {
            get
            {
                return rawMessage;
            }
        }
    }
}
