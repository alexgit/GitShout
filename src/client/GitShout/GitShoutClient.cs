﻿// -----------------------------------------------------------------------
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
        
        private ICollection<Action<CommitMessage>> actions = new LinkedList<Action<CommitMessage>>();
        
        public GitShoutClient(string server, int port)
        {            
            MessageProcessed += ReadMessageHeader;
            var client = new TcpClient(server, port);
            
            netStream = client.GetStream();
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
            messageBuffer = new byte[messageLength];
            netStream.BeginRead(messageBuffer, 0, messageBuffer.Length, OnBodyRead, null);
        }

        private void OnBodyRead(IAsyncResult result)
        {
            netStream.EndRead(result);
            ProcessActions();
            MessageProcessed(this, null);
        }

        public void OnCommit(Action<CommitMessage> action)
        {
            actions.Add(action);
        }

        private void ProcessActions()
        {
            var payload = Encoding.UTF8.GetString(messageBuffer);
            var commitMessage = JsonConvert.DeserializeObject<CommitMessage>(payload);

            foreach (var action in actions)
            {
                action(commitMessage);
            }
        }

        public void Start()
        {
            Listen();
        }        
    }
}
