using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace DevInbox
{

    public class DevInboxClient : IDevInbox
    {
        private static readonly Lazy<DevInboxClient> _instance =
            new Lazy<DevInboxClient>(() => new DevInboxClient(DefaultApiKey, DefaultApiUrl));

        private static string DefaultApiKey { get; set; } = string.Empty;
        private static string DefaultApiUrl { get; set; } = "https://api.devinbox.io/";

        private readonly HttpClient _httpClient;
        public const string SmtpServer = "smtp.devinbox.io";
        public const int SmtpServerPort = 587;

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        // Private constructor
        private DevInboxClient(string apiKey, string apiUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiUrl)
            };

            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            /*
             * Identification
             */
            var productValue = new ProductInfoHeaderValue("devinbox-sdk", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0");
            var dotnetValue = new ProductInfoHeaderValue("dotnet", Environment.Version.ToString());

            _httpClient.DefaultRequestHeaders.UserAgent.Add(productValue);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(dotnetValue);
        }

        public static void Initialize(string apiKey, string apiUrl = "https://api.devinbox.io/")
        {
            DefaultApiKey = apiKey;
            DefaultApiUrl = apiUrl;
        }

        public static IDevInbox Instance => _instance.Value;

        async Task<Mailbox> IDevInbox.CreateMailbox()
        {
            // Create the StringContent with the JSON payload
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await _httpClient.PostAsync("/mailboxes", content);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Deserialize the response content to the Mailbox object
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Mailbox>(responseContent, _jsonSerializerOptions) ?? throw new InvalidProgramException("Unable to deserialize response from server");
        }

        async Task IDevInbox.CreateMailbox(CreateMailboxOptions options)
        {
            var json = JsonSerializer.Serialize(options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/mailboxes", content);

            response.EnsureSuccessStatusCode();
        }

        async Task<MessageCountResult> IDevInbox.GetMessageCount(string mailboxKey)
        {
            var response = await _httpClient.GetAsync($"/messages/{mailboxKey}/count");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<MessageCountResult>(responseContent, _jsonSerializerOptions) ?? throw new InvalidProgramException("Unable to deserialize response from server");
        }

        async Task<MessageList> IDevInbox.GetMessages(string mailboxKey, int skip, int take)
        {
            var response = await _httpClient.GetAsync($"/messages/{mailboxKey}?skip={skip}&take={take}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<MessageList>(responseContent, _jsonSerializerOptions) ?? throw new InvalidProgramException("Unable to deserialize response from server");
        }

        async Task<Message> IDevInbox.GetLastMessage(string mailboxKey)
        {
            var response = await _httpClient.GetAsync($"/messages/{mailboxKey}/last");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Message>(responseContent, _jsonSerializerOptions) ?? throw new InvalidProgramException("Unable to deserialize response from server");
        }

        async Task<Message> IDevInbox.GetSingleMessage(string mailboxKey)
        {
            var response = await _httpClient.GetAsync($"/messages/{mailboxKey}/single");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Message>(responseContent, _jsonSerializerOptions) ?? throw new InvalidProgramException("Unable to deserialize response from server");
        }
    }
}
