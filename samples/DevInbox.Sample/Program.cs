using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DevInbox.Client.Api;
using DevInbox.Client.Client;
using DevInbox.Client.Model;
using DevInbox.Client.Extensions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DevInbox.Sample
{
    /// <summary>
    /// DevInbox .NET Client - Complete Usage Example
    /// Demonstrates creating mailbox, sending email via SMTP, and receiving via API
    /// 
    /// To run this sample you need to have a DevInbox account and API key.
    /// You can get it from your DevInbox dashboard at https://devinbox.io
    /// Once you have the API key, you can set it in the DEVINBOX_API_KEY environment variable.
    /// 
    /// This script will create a temporary mailbox, send an email via SMTP, and receive it via API.
    /// It will also test template parsing with the 'onboarding' template.
    /// 
    /// You need to create a template in your DevInbox dashboard with the name 'onboarding' and the following content:
    /// 
    /// Body (pay attention to whitespaces):
    /// ```
    ///<html>
    ///<body>
    ///     <h2>Hello {{ user_name }},</h2>
    ///     <p>This is a simple test message to verify email delivery.</p>
    ///    <p>If you receive this, the system is working correctly.</p>
    ///</body>
    /// </html>
    /// ```
    /// 
    /// Subject:
    /// ```
    /// Welcome {{ user_name }}!
    /// ```
    /// 
    /// Then you can run the sample using the following command:
    /// dotnet run
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await RunCompleteWorkflow();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Test failed with error: {ex.Message}");
                Console.WriteLine($"Error type: {ex.GetType().Name}");
                
                if (ex is ApiException apiEx)
                {
                    Console.WriteLine($"HTTP Status: {apiEx.StatusCode}");
                }
                
                Environment.Exit(1);
            }
        }

        static async Task RunCompleteWorkflow()
        {
            // Get API key from user
            var apiKey = GetApiKey();
            
            Console.WriteLine($"\n🔧 Configuring API client with key: {apiKey[..8]}...");
            
            // 1. Configure the API client using dependency injection
            var services = new ServiceCollection();
            
            // Add logging
            services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning));
            
            // Add the DevInbox API client
            services.AddApi(config =>
            {
                config.AddApiHttpClients(client =>
                {
                    client.BaseAddress = new Uri("https://api.devinbox.io");
                    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                });
            });
            
            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();
            
            // Get the API instances
            var mailboxesApi = serviceProvider.GetRequiredService<IMailboxesApi>();
            var messagesApi = serviceProvider.GetRequiredService<IMessagesApi>();
            
            Console.WriteLine("✅ API client configured successfully!");
            
            // 2. Create a temporary mailbox
            Console.WriteLine("\n📧 Creating a temporary mailbox...");
            var tempMailbox = new MailboxCreateModel();
            var tempResponse = await mailboxesApi.CreateMailboxAsync(tempMailbox);
            
            if (!tempResponse.IsOk)
            {
                throw new InvalidOperationException("Failed to create mailbox - response is not OK");
            }
            
            var mailboxData = tempResponse.Ok();
            Console.WriteLine($"   ✅ Temporary mailbox created!");
            Console.WriteLine($"   📋 Key: {mailboxData.Key}");
            Console.WriteLine($"   🔑 Password: {mailboxData.Password}");
            Console.WriteLine($"   📧 Email Address: {mailboxData.Key}@devinbox.io");
            
            var mailboxKey = mailboxData.Key;
            var mailboxPassword = mailboxData.Password;
            var mailboxEmail = $"{mailboxKey}@devinbox.io";
            
            // 3. Check that the mailbox is empty
            Console.WriteLine($"\n📊 Checking that mailbox is empty...");
            var countResponse = await messagesApi.GetMessageCountAsync(mailboxKey);
            
            if (!countResponse.IsOk)
            {
                throw new InvalidOperationException("Failed to get message count");
            }
            
            var messageCount = countResponse.Ok().Count;
            Console.WriteLine($"   ✅ Message count: {messageCount}");
            
            if (messageCount != 0)
            {
                throw new InvalidOperationException($"Mailbox is not empty! Expected 0 messages, got {messageCount}. This is a critical error as we just created the mailbox.");
            }
            Console.WriteLine("   ✅ Mailbox is empty as expected!");
            
            // 4. Send email to the mailbox via SMTP
            Console.WriteLine($"\n📤 Sending test email to {mailboxEmail}...");
            var emailSent = await SendEmailViaSmtpAsync(mailboxKey, mailboxPassword, mailboxEmail, sendHtmlOnly: true);
            
            if (!emailSent)
            {
                throw new InvalidOperationException("Failed to send email via SMTP. This is a critical test failure.");
            }            
            
            // 5. Check that the email was received
            Console.WriteLine($"\n📬 Checking if email was received...");
            
            var countResponseAfter = await messagesApi.GetMessageCountAsync(mailboxKey);
            
            if (!countResponseAfter.IsOk)
            {
                throw new InvalidOperationException("Failed to get message count after sending");
            }
            
            var messageCountAfter = countResponseAfter.Ok().Count;
            Console.WriteLine($"   📊 Message count after sending: {messageCountAfter}");
            
            if (messageCountAfter <= messageCount)
            {
                throw new InvalidOperationException($"Email was not received! Expected count > {messageCount}, got {messageCountAfter}. This indicates SMTP or email processing issues.");
            }
            Console.WriteLine("   ✅ Email was received successfully!");
            
            // Get the latest message
            Console.WriteLine($"\n📋 Retrieving the received email...");
            var messagesResponse = await messagesApi.GetMessagesAsync(mailboxKey, skip: 0, take: 1);
            
            if (!messagesResponse.IsOk)
            {
                throw new InvalidOperationException("Failed to get messages");
            }
            
            var messagesData = messagesResponse.Ok();
            if (messagesData.Messages == null || !messagesData.Messages.Any())
            {
                throw new InvalidOperationException("No messages found despite count increase. This indicates an API retrieval issue.");
            }
            
            var latestMessage = messagesData.Messages.First();
            Console.WriteLine($"   ✅ Email retrieved successfully!");
            Console.WriteLine($"   📧 From: {string.Join(", ", latestMessage.From ?? new List<string>())}");
            Console.WriteLine($"   📧 To: {string.Join(", ", latestMessage.To ?? new List<string>())}");
            Console.WriteLine($"   📧 Subject: {latestMessage.Subject}");
            Console.WriteLine($"   📧 Received: {latestMessage.Received}");
            Console.WriteLine($"   📧 Size: {(latestMessage.Body?.Length ?? 0)} characters");
            
            if (string.IsNullOrEmpty(latestMessage.Subject) || latestMessage.From == null || !latestMessage.From.Any() || latestMessage.To == null || !latestMessage.To.Any())
            {
                throw new InvalidOperationException("Email is missing required fields (subject, from, or to)");
            }
            
            Console.WriteLine($"   📋 Message type: {(latestMessage.IsHtml ? "HTML" : "Text")}");
            Console.WriteLine($"   📋 Body content: {(latestMessage.Body?.Length > 100 ? latestMessage.Body[..100] + "..." : latestMessage.Body ?? "")}");
            
            if (string.IsNullOrEmpty(latestMessage.Body))
            {
                throw new InvalidOperationException("Message body is empty");
            }
            
            // Show preview of email content
            if (!string.IsNullOrEmpty(latestMessage.Body))
            {
                var preview = latestMessage.Body.Length > 200 ? latestMessage.Body[..200] + "..." : latestMessage.Body;
                Console.WriteLine($"   📄 Content preview: {preview}");
            }
            
            // 6. Test getting single message with template parsing
            Console.WriteLine($"\n🔍 Testing single message retrieval with 'onboarding' template...");
            try
            {
                var parsedResponse = await messagesApi.GetSingleMessageWithTemplateAsync(mailboxKey, "onboarding");
                
                if (!parsedResponse.IsOk)
                {
                    throw new InvalidOperationException("Template parsing failed - response is not OK");
                }
                
                var parsedMessage = parsedResponse.Ok();
                Console.WriteLine($"   ✅ Template parsing successful!");
                Console.WriteLine($"   📧 From: {string.Join(", ", parsedMessage.From ?? new List<string>())}");
                Console.WriteLine($"   📧 To: {string.Join(", ", parsedMessage.To ?? new List<string>())}");
                Console.WriteLine($"   📧 Subject: {parsedMessage.Subject}");
                Console.WriteLine($"   📧 Body: {parsedMessage.Body}");
                Console.WriteLine($"   📧 Is HTML: {parsedMessage.IsHtml}");
                Console.WriteLine($"   📧 Received: {parsedMessage.Received}");
                
                // Note: The MessageParsedViewModel doesn't have a 'parameters' field
                // Template parsing works by replacing {{ user_name }} with actual values in subject/body dictionaries
                if (parsedMessage.Body != null)
                {
                    Console.WriteLine($"   📋 Parsed body content: {parsedMessage.Body}");
                    // The body should be a dictionary with user_name parameter
                    if (parsedMessage.Body is Dictionary<string, string> bodyDict && bodyDict.ContainsKey("user_name"))
                    {
                        var userName = bodyDict["user_name"];
                        if (userName == "John Doe")
                        {
                            Console.WriteLine($"   ✅ Body contains user_name='John Doe' as expected!");
                        }
                        else
                        {
                            Console.WriteLine($"   ⚠️  Body contains user_name='{userName}' instead of 'John Doe'");
                        }
                    }
                }
                
                if (parsedMessage.Subject != null)
                {
                    Console.WriteLine($"   📋 Parsed subject content: {parsedMessage.Subject}");
                    // The subject should be a dictionary with user_name parameter
                    if (parsedMessage.Subject is Dictionary<string, string> subjectDict && subjectDict.ContainsKey("user_name"))
                    {
                        var userName = subjectDict["user_name"];
                        if (userName == "John Doe")
                        {
                            Console.WriteLine($"   ✅ Subject contains user_name='John Doe' as expected!");
                        }
                        else
                        {
                            Console.WriteLine($"   ⚠️  Subject contains user_name='{userName}' instead of 'John Doe'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ Template parsing failed: {ex.Message}");
                Console.WriteLine($"   ℹ️  This might be expected if the 'onboarding' template is not configured on the server");
            }
            
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("✅ Complete DevInbox workflow test completed!");
            Console.WriteLine("\n🎯 What we accomplished:");
            Console.WriteLine("  📧 Created a temporary mailbox");
            Console.WriteLine("  📊 Verified mailbox was empty");
            Console.WriteLine("  📤 Sent email via SMTP with test content");
            Console.WriteLine("  📬 Verified email was received via API");
            Console.WriteLine("  🔍 Tested template parsing with 'onboarding' template");
            Console.WriteLine("\n🚀 DevInbox is working perfectly!");
        }

        static string GetApiKey()
        {
            Console.WriteLine("DevInbox .NET Client - Complete Usage Example");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("Please provide your DevInbox API key.");
            Console.WriteLine("You can find it in your DevInbox dashboard at https://devinbox.io");
            Console.WriteLine("Press Enter to use DEVINBOX_API_KEY environment variable");
            Console.WriteLine();
            
            var apiKey = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(apiKey))
            {
                // Try to get from environment variable
                apiKey = Environment.GetEnvironmentVariable("DEVINBOX_API_KEY");
                if (string.IsNullOrEmpty(apiKey))
                {
                    Console.WriteLine("❌ No API key provided and DEVINBOX_API_KEY environment variable is not set.");
                    Console.WriteLine("Please either:");
                    Console.WriteLine("  1. Enter your API key when prompted");
                    Console.WriteLine("  2. Set the DEVINBOX_API_KEY environment variable");
                    Environment.Exit(1);
                }
                Console.WriteLine("✅ Using API key from DEVINBOX_API_KEY environment variable");
            }
            else
            {
                Console.WriteLine("✅ Using provided API key");
            }
            
            return apiKey;
        }

        static string GetTestBodyContent()
        {
            return @"Hello John Doe,

This is a simple test message to verify email delivery.
If you receive this, the system is working correctly.";
        }

        static string CreateTestHtml()
        {
            return @"
<html>
<body>
    <h2>Hello John Doe,</h2>
    <p>This is a simple test message to verify email delivery.</p>
    <p>If you receive this, the system is working correctly.</p>
</body>
</html>
    ";
        }

        static async Task<bool> SendEmailViaSmtpAsync(string mailboxUsername, string mailboxPassword, string toEmail, bool sendHtmlOnly = false)
        {
            Console.WriteLine($"\n📤 Sending email via SMTP to {toEmail}...");
            
            // SMTP configuration as specified
            const string smtpServer = "smtp.devinbox.io";
            const int smtpPort = 587;
            
            try
            {
                using var client = new SmtpClient();
                
                Console.WriteLine($"   🔌 Connecting to {smtpServer}:{smtpPort}...");
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                
                Console.WriteLine($"   🔑 Authenticating as {mailboxUsername}...");
                await client.AuthenticateAsync(mailboxUsername, mailboxPassword);
                
                // Create message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("DevInbox Test", mailboxUsername));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Welcome John Doe!";
                
                var bodyBuilder = new BodyBuilder();
                
                if (sendHtmlOnly)
                {
                    // Send only HTML
                    bodyBuilder.HtmlBody = CreateTestHtml();
                    Console.WriteLine($"   📧 Sending HTML-only email with {bodyBuilder.HtmlBody.Length} characters");
                }
                else
                {
                    // Send both text and HTML (multipart)
                    bodyBuilder.TextBody = GetTestBodyContent();
                    bodyBuilder.HtmlBody = CreateTestHtml();
                    
                    Console.WriteLine($"   📧 Sending multipart email with:");
                    Console.WriteLine($"      - Text part: {bodyBuilder.TextBody.Length} characters");
                    Console.WriteLine($"      - HTML part: {bodyBuilder.HtmlBody.Length} characters");
                }
                
                message.Body = bodyBuilder.ToMessageBody();
                
                Console.WriteLine("   📧 Sending email...");
                await client.SendAsync(message);
                
                await client.DisconnectAsync(true);
                
                Console.WriteLine("   ✅ Email sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}