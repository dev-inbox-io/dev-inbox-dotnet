# DevInbox.Client.Model.MessageParsedViewModel

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**UniqueId** | **Guid** | Unique identifier for the message | 
**IsHtml** | **bool** | Whether the email body is HTML format | 
**Received** | **DateTime** | Timestamp when the message was received | 
**From** | **List&lt;string&gt;** | Array of sender email addresses | 
**To** | **List&lt;string&gt;** | Array of recipient email addresses | 
**Cc** | **List&lt;string&gt;** | Array of CC email addresses | 
**Bcc** | **List&lt;string&gt;** | Array of BCC email addresses | 
**Subject** | **Dictionary&lt;string, string&gt;** | Parsed subject template parameters | 
**Body** | **Dictionary&lt;string, string&gt;** | Parsed body template parameters | 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

