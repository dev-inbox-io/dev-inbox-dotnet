using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevInbox
{
    public class MailboxCreateModel
    {
        public string Name { get; set; }

        public string Project { get; set; }

    }

    public class Mailbox
    {
        public string Key { get; set; }
        public string Password { get; set; }
    }

    public class MessageCountResult
    {
        public int Count { get; set; }
    }

    public class Message
    {
        public Guid UniqueId { get; set; }

        public string[] From { get; set; }

        public string[] To { get; set; }

        public string[] Cc { get; set; }

        public string[] Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsHtml { get; set; }

        public DateTime Received { get; set; }
    }

    public class MessageList
    {
        public string Key { get; set; }

        public int Count { get; set; }

        public Message[] Messages { get; set; }
    }
}
