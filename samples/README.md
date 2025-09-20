# DevInbox .NET Samples

This directory contains sample projects demonstrating how to use the DevInbox .NET client library.

## Projects

### DevInbox.Sample

A comprehensive console application that demonstrates the complete DevInbox workflow:

- Creating temporary mailboxes
- Sending emails via SMTP using MailKit
- Receiving emails via the DevInbox API
- Template parsing functionality

## Running the Sample

1. Navigate to the sample project:
   ```bash
   cd DevInbox.Sample
   ```

2. Set your DevInbox API key:
   ```bash
   # Option 1: Environment variable
   set DEVINBOX_API_KEY=your_api_key_here
   
   # Option 2: Enter when prompted
   dotnet run
   ```

3. Run the sample:
   ```bash
   dotnet run
   ```

## Prerequisites

- .NET 8.0 or later
- A DevInbox account and API key from [https://devinbox.io](https://devinbox.io)
- The DevInbox API service running on `http://localhost:5062`

## Solution Structure

This directory contains its own solution file (`DevInbox.Samples.sln`) that includes:
- The sample project
- A reference to the main DevInbox.Client library

This keeps the samples separate from the main client library solution, allowing for independent development and testing.

## Building

To build all samples:

```bash
dotnet build
```

To build a specific sample:

```bash
dotnet build DevInbox.Sample
```
