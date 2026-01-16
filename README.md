# DevInbox .NET Client SDK

[![NuGet](https://img.shields.io/nuget/v/DevInbox.Client.svg)](https://www.nuget.org/packages/DevInbox.Client/)
[![Build Status](https://github.com/dev-inbox-io/dev-inbox-dotnet/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dev-inbox-io/dev-inbox-dotnet/actions)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)

A .NET client library for the [DevInbox](https://devinbox.io) API - a service for creating test mailboxes and managing email messages during development and testing.

## Features

- ✅ Create temporary or persistent mailboxes
- ✅ Receive and manage test emails via API
- ✅ Parse email templates with parameter extraction
- ✅ Full async/await support
- ✅ Built-in retry policies and circuit breakers
- ✅ Dependency injection ready
- ✅ Strongly typed models

## Installation

Install the package from NuGet:

```bash
dotnet add package DevInbox.Client
```

Or via Package Manager:

```powershell
Install-Package DevInbox.Client
```

## Quick Start

### Basic Usage

```csharp
using DevInbox.Client.Api;
using DevInbox.Client.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var services = new ServiceCollection();

// Configure the API client
services.AddApi(config =>
{
    config.AddApiHttpClients(client =>
    {
        client.BaseAddress = new Uri("https://api.devinbox.io");
        client.DefaultRequestHeaders.Add("X-Api-Key", "your-api-key-here");
    });
});

var serviceProvider = services.BuildServiceProvider();
var mailboxesApi = serviceProvider.GetRequiredService<IMailboxesApi>();

// Create a temporary mailbox
var mailbox = new MailboxCreateModel();
var response = await mailboxesApi.CreateMailboxAsync(mailbox);

if (response.IsOk)
{
    var mailboxData = response.Ok();
    Console.WriteLine($"Mailbox created: {mailboxData.Key}@devinbox.io");
    Console.WriteLine($"Password: {mailboxData.Password}");
}
```

### Advanced Configuration with Retry Policies

```csharp
services.AddApi(config =>
{
    config.AddApiHttpClients(client =>
    {
        client.BaseAddress = new Uri("https://api.devinbox.io");
        client.DefaultRequestHeaders.Add("X-Api-Key", "your-api-key-here");
    }, builder =>
    {
        builder
            .AddRetryPolicy(2)
            .AddTimeoutPolicy(TimeSpan.FromSeconds(5))
            .AddCircuitBreakerPolicy(10, TimeSpan.FromSeconds(30));
    });
});
```

## Documentation

For detailed documentation and examples, see the [project README](src/DevInbox.Client/README.md).

### API Reference

- **MailboxesApi**: Create and manage mailboxes
- **MessagesApi**: Retrieve and parse email messages
- **StatusApi**: Check API service status

### Models

- `MailboxCreateModel`: Create a new mailbox (with optional name, project, and temporary flag)
- `MailboxViewModel`: Mailbox information (key, password, email address)
- `MessageViewModel`: Email message details
- `MessageParsedViewModel`: Parsed email with template parameters
- `MessageCountResult`: Count of messages in a mailbox

## Examples

See the [samples directory](samples/) for complete working examples including:
- Creating mailboxes
- Sending emails via SMTP
- Receiving emails via API
- Template parsing

## Requirements

- .NET 8.0 or later
- An API key from [DevInbox](https://devinbox.io)

## Getting Your API Key

1. Sign up at [https://devinbox.io](https://devinbox.io)
2. Navigate to your dashboard
3. Copy your API key
4. Use it in the `X-Api-Key` header when configuring the client

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or feature requests, please visit:
- [GitHub Issues](https://github.com/dev-inbox-io/dev-inbox-dotnet/issues)
- [DevInbox Support](https://devinbox.io/support)

## Related Projects

- [DevInbox Node.js SDK](https://github.com/dev-inbox-io/dev-inbox-nodejs)
- [DevInbox Python SDK](https://github.com/dev-inbox-io/dev-inbox-python)

---

**Note**: This SDK is automatically generated from the OpenAPI specification. For the latest API documentation, visit [https://api.devinbox.io/docs](https://api.devinbox.io/docs).
