using MailKit.Security;
using MimeKit;
using Shouldly;
using System.Net;
using System.Net.Mail;

namespace DevInbox.ApiTests
{
    [TestClass]
    public sealed class MailboxBasicTests
    {
        [TestMethod]
        public async Task CreateAnUnnamedMailboxAndTestUsingStandardSmtpClient()
        {
            // Arrange
            DevInboxClient.Initialize(Environment.GetEnvironmentVariable("DEVINBOX_API_KEY"));
            
            var mailbox = await DevInboxClient.Instance.CreateMailbox();

            mailbox.ShouldNotBeNull();
            mailbox.Key.ShouldNotBeNullOrWhiteSpace();
            mailbox.Password.ShouldNotBeNullOrWhiteSpace();

            //send an email to the mailbox
            var smtpClient = new SmtpClient(DevInboxClient.SmtpServer, DevInboxClient.SmtpServerPort)
            {
                Credentials = new NetworkCredential(mailbox.Key, mailbox.Password),
                EnableSsl = true
            };

            // Create the email message
            var mailMessage = new MailMessage
            {
                From = new MailAddress("from@dummy.com"),
                Subject = "Test Email",
                Body = "This is a test email sent to the mailbox.",
                IsBodyHtml = false
            };

            // Add the mailbox's email address as the recipient
            mailMessage.To.Add("to@dummy.com");

            // Act
            await smtpClient.SendMailAsync(mailMessage);

            // Assert
            var countResult = await DevInboxClient.Instance.GetMessageCount(mailbox.Key);
            countResult.ShouldNotBeNull();
            countResult.Count.ShouldBe(1);

            var messages = await DevInboxClient.Instance.GetMessages(mailbox.Key);
            messages.ShouldNotBeNull();
            messages.Key.ShouldBe(mailbox.Key);
            messages.Count.ShouldBe(1);
            messages.Messages.ShouldNotBeNull();
            messages.Messages.Length.ShouldBe(1);
            messages.Messages[0].From.ShouldNotBeNull();
            messages.Messages[0].From.Length.ShouldBe(1);
            messages.Messages[0].From[0].ShouldBe("from@dummy.com");

            messages.Messages[0].To.ShouldNotBeNull();
            messages.Messages[0].To.Length.ShouldBe(1);
            messages.Messages[0].To[0].ShouldBe("to@dummy.com");

            messages.Messages[0].Subject.ShouldBe("Test Email");
            messages.Messages[0].Body.ShouldBe("This is a test email sent to the mailbox.");
        }

        [TestMethod]
        public async Task CreateAnUnnamedMailboxAndTestUsingStandardMailKitSmtpClient()
        {
            // Arrange
            DevInboxClient.Initialize(Environment.GetEnvironmentVariable("DEVINBOX_API_KEY"));

            var mailbox = await DevInboxClient.Instance.CreateMailbox();

            mailbox.ShouldNotBeNull();
            mailbox.Key.ShouldNotBeNullOrWhiteSpace();
            mailbox.Password.ShouldNotBeNullOrWhiteSpace();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("From", "from@dummy.com"));
            message.To.Add(new MailboxAddress("To", "to@dummy.com"));
            message.Subject = "Test Email";

            // Create body with explicit line endings control
            var textPart = new TextPart("plain")
            {
                Text = "This is a test email sent to the mailbox."
            };

            message.Body = textPart;

            // Send using MailKit's SmtpClient
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(DevInboxClient.SmtpServer, DevInboxClient.SmtpServerPort, SecureSocketOptions.StartTls);
                client.Authenticate(mailbox.Key, mailbox.Password);
                client.Send(message);
                client.Disconnect(true);
            }

            // Assert
            var countResult = await DevInboxClient.Instance.GetMessageCount(mailbox.Key);
            countResult.ShouldNotBeNull();
            countResult.Count.ShouldBe(1);

            var messages = await DevInboxClient.Instance.GetMessages(mailbox.Key);
            messages.ShouldNotBeNull();
            messages.Key.ShouldBe(mailbox.Key);
            messages.Count.ShouldBe(1);
            messages.Messages.ShouldNotBeNull();
            messages.Messages.Length.ShouldBe(1);
            messages.Messages[0].From.ShouldNotBeNull();
            messages.Messages[0].From.Length.ShouldBe(1);
            messages.Messages[0].From[0].ShouldBe("from@dummy.com");

            messages.Messages[0].To.ShouldNotBeNull();
            messages.Messages[0].To.Length.ShouldBe(1);
            messages.Messages[0].To[0].ShouldBe("to@dummy.com");

            messages.Messages[0].Subject.ShouldBe("Test Email");
            messages.Messages[0].Body.ShouldBe("This is a test email sent to the mailbox.");
        }

    }
}
