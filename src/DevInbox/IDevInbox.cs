using System.Threading.Tasks;

namespace DevInbox
{
    public interface IDevInbox
    {
        Task<Mailbox> CreateMailbox();

        Task CreateMailbox(CreateMailboxOptions options);

        Task<MessageCountResult> GetMessageCount(string mailboxKey);

        Task<MessageList> GetMessages(string mailboxKey, int skip, int take);

        Task<Message> GetLastMessage(string mailboxKey);

        Task<Message> GetSingleMessage(string mailboxKey);
    }

    public static class DevInboxClientExtensions
    {
        public static Task CreateMailbox(this IDevInbox devInbox, string name, string project)
        {
            return devInbox.CreateMailbox(new CreateMailboxOptions
            {
                Name = name,
                Project = project
            });
        }

        public static Task<MessageList> GetMessages(this IDevInbox devInbox, string mailboxKey)
        {
            return devInbox.GetMessages(mailboxKey, 0, 10);
        }

    }

    public class CreateMailboxOptions
    {
        public string Name { get; set; }

        public string Project { get; set; }
    }
}
