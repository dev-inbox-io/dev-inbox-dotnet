using System.Threading.Tasks;

namespace DevInbox
{
    public interface IDevInbox
    {
        Task<Mailbox> CreateMailbox();

        Task CreateMailbox(CreateMailboxOptions options);

        Task<MessageCountResult> GetMessageCount(string mailboxKey);

        Task<MessageList> GetMessages(string mailboxKey, int skip = 0, int take = 10);
    }

    public static class DevInboxClientExtensions
    {
        public static Task CreateMailbox(IDevInbox devInbox, string name, string project)
        {
            return devInbox.CreateMailbox(new CreateMailboxOptions
            {
                Name = name,
                Project = project
            });
        }
    }

    public class CreateMailboxOptions
    {
        public string Name { get; set; }

        public string Project { get; set; }

        public LineEndingHandling LineEndingMode { get; set; } = LineEndingHandling.Normalize;
    }
}
