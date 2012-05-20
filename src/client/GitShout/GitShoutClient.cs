// -----------------------------------------------------------------------
// <copyright file="GitShoutClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;
using Newtonsoft.Json;

namespace GitShout
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GitShoutClient
    {
        private readonly NetworkStream netStream;
        
        /// <summary>
        /// The message header contains the length of the message that follows it.
        /// The the 4 byte array contains a little endian integer representation of the length.
        /// </summary>
        private readonly byte[] messageHeader = new byte[4];
        private byte[] messageBuffer;
        private event MessageProcessedEventHandler MessageProcessed;
        private delegate void MessageProcessedEventHandler(object sender, EventArgs args);
        
        private ICollection<Action<CommitMessage>> actions = new LinkedList<Action<CommitMessage>>();
        
        public GitShoutClient(string server, int port)
        {
            MessageProcessed += ReadMessageHeader;
            var client = new TcpClient(server, port);
            
            netStream = client.GetStream();
        }

        public void Start()
        {
            Listen();
        }

        private void Listen()
        {
            ReadMessageHeader(this, null);
        }

        private void ReadMessageHeader(object source, EventArgs args)
        {
            netStream.BeginRead(messageHeader, 0, messageHeader.Length, OnMessageHeaderRead, null);
        }

        private void OnMessageHeaderRead(IAsyncResult result)
        {
            netStream.EndRead(result);
            var messageLength = BitConverter.ToInt32(messageHeader, 0);

            ReadMessageBody(messageLength);
        }

        private void ReadMessageBody(int messageLength)
        {
            var readState = new ReadState(messageLength);
            messageBuffer = new byte[messageLength];
            netStream.BeginRead(messageBuffer, 0, messageBuffer.Length, OnChunkRead, readState);
        }

        private void OnChunkRead(IAsyncResult result)
        {
            var bytesRead = netStream.EndRead(result);
            var readState = (ReadState)result.AsyncState;
            readState.BytesRead = bytesRead;

            if (!readState.Finished)
            {
                ContinueReadMessageBody(readState);
                return;
            }

            RunActions();
            MessageProcessed(this, null);
        }

        private void ContinueReadMessageBody(ReadState readState)
        {
            netStream.BeginRead(messageBuffer, readState.BytesRead, readState.BytesRemaining, OnChunkRead, readState);
        }
                
        public void OnCommit(Action<CommitMessage> action)
        {
            actions.Add(action);
        }

        private void RunActions()
        {
            var payload = Encoding.UTF8.GetString(messageBuffer);

            CommitMessage commitMessage;
            try
            {
                commitMessage = JsonConvert.DeserializeObject<CommitMessage>(payload);                
            } 
            catch(Exception e) 
            {
                throw new InvalidMessageFormatException("The message was not a valid JSON string.", payload, e);                
            }

            foreach (var action in actions)
            {
                action(commitMessage);
            }
        }        

        private class ReadState 
        {
            public ReadState(int messageSize) 
            {
                MessageSize = messageSize;
            }

            public int MessageSize { get; private set; }
            public int BytesRemaining { get { return MessageSize - BytesRead; } }
            public int BytesRead { get; set; }
            public bool Finished { get { return BytesRemaining == 0; } }
        }
    }
}
