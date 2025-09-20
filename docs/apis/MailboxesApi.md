# DevInbox.Client.Api.MailboxesApi

All URIs are relative to *http://localhost:5062*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateMailbox**](MailboxesApi.md#createmailbox) | **POST** /mailboxes | Create a new mailbox |

<a id="createmailbox"></a>
# **CreateMailbox**
> MailboxViewModel CreateMailbox (MailboxCreateModel mailboxCreateModel)

Create a new mailbox

Creates a new mailbox for receiving test emails. If no name is provided, a temporary mailbox is created.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **mailboxCreateModel** | [**MailboxCreateModel**](MailboxCreateModel.md) |  |  |

### Return type

[**MailboxViewModel**](MailboxViewModel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

