# DevInbox.Client.Api.MessagesApi

All URIs are relative to *http://localhost:5062*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetLastMessage**](MessagesApi.md#getlastmessage) | **GET** /messages/{key}/last | Get the last message from a mailbox |
| [**GetLastMessageWithTemplate**](MessagesApi.md#getlastmessagewithtemplate) | **GET** /messages/{key}/{template}/last | Get the last message with template parsing |
| [**GetMessageById**](MessagesApi.md#getmessagebyid) | **GET** /messages/{key}/get | Get a specific message by ID |
| [**GetMessageCount**](MessagesApi.md#getmessagecount) | **GET** /messages/{key}/count | Get message count for a mailbox |
| [**GetMessages**](MessagesApi.md#getmessages) | **GET** /messages/{key} | Get messages from a mailbox |
| [**GetSingleMessage**](MessagesApi.md#getsinglemessage) | **GET** /messages/{key}/single | Get single message from a mailbox |
| [**GetSingleMessageWithTemplate**](MessagesApi.md#getsinglemessagewithtemplate) | **GET** /messages/{key}/{template}/single | Get single message with template parsing |

<a id="getlastmessage"></a>
# **GetLastMessage**
> MessageViewModel GetLastMessage (string key)

Get the last message from a mailbox

Retrieves the most recent message from the specified mailbox. Returns an error if the mailbox is empty.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **key** | **string** |  |  |

### Return type

[**MessageViewModel**](MessageViewModel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getlastmessagewithtemplate"></a>
# **GetLastMessageWithTemplate**
> MessageParsedViewModel GetLastMessageWithTemplate (string key, string template)

Get the last message with template parsing

Retrieves the most recent message from the specified mailbox and parses it using the provided template. Returns an error if the mailbox is empty.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **key** | **string** |  |  |
| **template** | **string** |  |  |

### Return type

[**MessageParsedViewModel**](MessageParsedViewModel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getmessagebyid"></a>
# **GetMessageById**
> MessageViewModel GetMessageById (string key, Guid id)

Get a specific message by ID

Retrieves a specific message from the specified mailbox using its unique ID.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **key** | **string** |  |  |
| **id** | **Guid** |  |  |

### Return type

[**MessageViewModel**](MessageViewModel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getmessagecount"></a>
# **GetMessageCount**
> MessageCountResult GetMessageCount (string key)

Get message count for a mailbox

Returns the total number of messages in the specified mailbox.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **key** | **string** |  |  |

### Return type

[**MessageCountResult**](MessageCountResult.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getmessages"></a>
# **GetMessages**
> MessagesViewModel GetMessages (string key, int skip = null, int take = null)

Get messages from a mailbox

Retrieves messages from the specified mailbox with optional pagination. Default: skip=0, take=10, max take=25.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **key** | **string** |  |  |
| **skip** | **int** |  | [optional]  |
| **take** | **int** |  | [optional]  |

### Return type

[**MessagesViewModel**](MessagesViewModel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getsinglemessage"></a>
# **GetSingleMessage**
> MessageViewModel GetSingleMessage (string key)

Get single message from a mailbox

Retrieves a single message from the specified mailbox. Returns an error if the mailbox contains 0 or more than 1 message.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **key** | **string** |  |  |

### Return type

[**MessageViewModel**](MessageViewModel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getsinglemessagewithtemplate"></a>
# **GetSingleMessageWithTemplate**
> MessageParsedViewModel GetSingleMessageWithTemplate (string key, string template)

Get single message with template parsing

Retrieves a single message from the specified mailbox and parses it using the provided template. Returns an error if the mailbox contains 0 or more than 1 message.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **key** | **string** |  |  |
| **template** | **string** |  |  |

### Return type

[**MessageParsedViewModel**](MessageParsedViewModel.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **403** | Forbidden |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

