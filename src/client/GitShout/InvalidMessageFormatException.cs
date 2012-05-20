using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitShout
{
    public class InvalidMessageFormatException : Exception
    {
        private readonly string messagePayload;

        public InvalidMessageFormatException(string message, string messagePayload, Exception innerException) : base(message, innerException) 
        {
            this.messagePayload = messagePayload;
        }

        public InvalidMessageFormatException(string message, string messagePayload) : base(message) 
        {
            this.messagePayload = messagePayload;
        }

        public string MessagePayload
        {
            get { return messagePayload; }
        }
    }
}
