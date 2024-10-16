---
# Metadata required by https://docs.microsoft.com/samples/browse/
# Metadata properties: https://review.docs.microsoft.com/help/contribute/samples/process/onboarding?branch=main#add-metadata-to-readme
languages:
- csharp
page_type: sample
name: ASP.NET Core minimal web API that protects API
description: This ASP.NET Core minimal web API protects an API endpoint. The code in this sample is used by one or more articles on docs.microsoft.com.
products:
- azure
- entra-id
- ms-graph
urlFragment: ms-identity-docs-code-web-apicsharp
---

# ASP.NET Core minimal web API | web API | access control (protected routes) | Microsoft identity platform

The sample code provided here has been created using minimal web API in ASP.NET Core 6.0, and slightly modified to be protected for a single organization using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) that interacts with [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview).  In other words, a very minimalist web api is secured by adding an authorization layer before user requests can reach protected resources.  At this point it is expected that the user sign-in had already happened, so api calls can be made in the name of the signed-in user. For that to be possible a token containing user's information is being sent in the request headers and used in the authorization process.

> :page_with_curl: This sample application backs one or more technical articles on docs.microsoft.com.

## Prerequisites

- A Microsoft Entra tenant. You can [open an Azure account for free](https://azure.microsoft.com/free) to get a Microsoft Entra instance.
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Setup

### 1. Register the web API

First, complete the steps in [Quickstart: Configure an application to expose a web API](https://learn.microsoft.com/azure/active-directory/develop/quickstart-configure-app-expose-web-apis) to register the web API with the identity platform and configure its scopes.

Use the following settings for your web API's app registration:

| App registration <br/> setting | Value for this sample app | Notes |
|:-:|:-|:-|
| **Name** | `web-api-aspnetcore-minimal` | Suggested value for this sample. <br/><br/> You can change this value later without impacting application functionality. |
| **Supported account types** | **Accounts in this organizational directory only (Single tenant)** | Required for this sample. <br/><br/> Tells the identity platform which identities this application supports; affects how security tokens like ID and access tokens are requested, formatted, and issued. |
| **Application ID URI** | `api://{APPLICATION_CLIENT_ID}` | Suggested value for this sample. <br/><br/>  Replace `{APPLICATION_CLIENT_ID}` with the web API's **Application (client) ID**. |

> :information_source: **Bold text** refers to a UI element in the Microsoft Entra admin center and `code formatting` indicates a value to enter or accept.

### 2. Add scopes

Add the following scopes by using **Expose an API** in the web API's app registration:

| Scope name      | Who can consent?     | Admin consent display name | Admin consent description                               | User consent display name | User consent description                                | State                 |
|-----------------|----------------------|----------------------------|---------------------------------------------------------|---------------------------|---------------------------------------------------------|-----------------------|
| `Forecast.Read` | **Admins and users** | `Read forecast data`       | `Allows the application to read weather forecast data.` | `Read forecast data`      | `Allows the application to read weather forecast data.` | **Enabled** (default) |

### 3. Configure the code

In the _./appsettings.json_ file, replace these `{PLACEHOLDER}` values with the corresponding values from your web API's app registration:

```json
"ClientId": "Enter the client ID obtained from the Microsoft Entra admin center",
"TenantId": "Enter the tenant ID obtained from the Microsoft Entra admin center",
```

For example:

```json
"ClientId": "00001111-aaaa-2222-bbbb-3333cccc4444",
"TenantId": "dddd5555-eeee-6666-ffff-00001111aaaa",
```

## Run the application

### 1. Run the web API

Execute the following command to get the app up and running:

```bash
dotnet run
```

### 2. Send an unauthenticated request to the web API

To verify the endpoint is protected, use cURL to send an unauthenticated HTTP GET request (including no access token) to the protected endpoint.

With no access token included in the request, the expected response is `401 Unauthorized` with output similar to this:

```bash
user@host:~$ curl -X GET https://localhost:5001/weatherforecast -ki
HTTP/2 401
date: Fri, 23 Sep 2022 23:34:24 GMT
server: Kestrel
www-authenticate: Bearer
content-length: 0
```

### 3. Send an authenticated request to the web API

Next, send an authenticated HTTP GET request (one that includes a valid access token) to the protected endpoint.

With a valid access token included in the request, the expected response is `200 OK` with output similar to this:

```bash
user@host:~/web-api$ curl -X GET https://localhost:5001/weatherforecast -ki \
-H 'Content-Type: application/json' \
-H "Authorization: Bearer {ACCESS_TOKEN}"

HTTP/2 200
content-type: application/json; charset=utf-8
date: Fri, 23 Sep 2022 23:34:47 GMT
server: Kestrel

[{"date":"2022-09-23T14:20:42.4772509-07:00","temperatureC":-17,"summary":"Freezing","temperatureF":2},{"date":"2022-09-24T14:20:42.4772803-07:00","temperatureC":-15,"summary":"Sweltering","temperatureF":6},{"date":"2022-09-25T14:20:42.4772819-07:00","temperatureC":51,"summary":"Balmy","temperatureF":123},{"date":"2022-09-26T14:20:42.4772832-07:00","temperatureC":34,"summary":"Chilly","temperatureF":93},{"date":"2022-09-27T14:20:42.4772846-07:00","temperatureC":-13,"summary":"Hot","temperatureF":9}]
```

## About the code

The base project for this code sample was generated by using the ASP.NET Core minimal web API project template.

Then, a reference to _Microsoft.Identity.Web_ was added by installing its NuGet package. _Microsoft.Identity.Web_, a wrapper library for MSAL.NET, adds a convenience layer to MSAL.NET that makes it easier to use in ASP.NET Core applications like this web API.

```bash
# Create ASP.NET minimal web API project
dotnet new webapi -minimal -o Api

# Add the Microsoft.Identity.Web assembly reference
dotnet add package Microsoft.Identity.Web
```

The web API's code uses Microsoft.Identity.Web and ASP.NET Core's policy-based authorization to protect a route endpoint:

1. Chain Microsoft.Identity.Web's [AddMicrosoftIdentityWebApi] extension method to the standard ASP.NET Core [WebApplicationBuilder] chain in _Startup.cs_.
1. Define a scope by using Microsoft.Identity.Web's [ScopeAuthorizationRequirement] when adding an ASP.NET Core authorization policy in builder chain. The scope's name is read from the _appsettings.json_ configuration file.
1. "Protect" the API (require authorization) by decorating a route endpoint with ASP.NET Core's `[authorize]` attribute. Specify the name of the authorization policy instantiated with the Microsoft.Identity.Web's [ScopeAuthorizationRequirement] as described in the previous step.

## Reporting problems

### Sample app not working?

If you can't get the sample working, you've checked [Stack Overflow](http://stackoverflow.com/questions/tagged/msal), and you've already searched the issues in this sample's repository, open an issue report the problem.

1. Search the [GitHub issues](../../issues) in the repository - your problem might already have been reported or have an answer.
1. Nothing similar? [Open an issue](../../issues/new) that clearly explains the problem you're having running the sample app.

### All other issues

> :warning: WARNING: Any issue _not_ limited to running this or another sample app will be closed without being addressed.

For all other requests, see [Support and help options for developers | Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/developer-support-help-options).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

<!-- LINKS -->
[AddMicrosoftIdentityWebApi]: https://learn.microsoft.com/dotnet/api/microsoft.identity.web.microsoftidentitywebapiauthenticationbuilderextensions.addmicrosoftidentitywebapi
[ScopeAuthorizationRequirement]: https://learn.microsoft.com/dotnet/api/microsoft.identity.web.scopeauthorizationrequirement
[WebApplicationBuilder]: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.webapplicationbuilder
