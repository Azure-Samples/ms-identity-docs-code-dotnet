<!-- UNCOMMENT YAML FRONT MATTER TO DISPLAY IN SAMPLES BROWSER
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
- azure-active-directory
- ms-graph
urlFragment: ms-identity-docs-code-csharp
---
-->
<!-- SAMPLE ID: DOCS-CODE-001 -->
# ASP.NET Core minimal web API | web API | access control (protected routes) | Microsoft identity platform

<!-- Build badges here
![Build passing.](https://img.shields.io/badge/build-passing-brightgreen.svg) ![Code coverage.](https://img.shields.io/badge/coverage-100%25-brightgreen.svg) ![License.](https://img.shields.io/badge/license-MIT-green.svg)
-->

The sample code provided here has been created using minimal web API in ASP.NET Core 6.0, and slightly modified to be protected for a single organization using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) that interacts with [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview).  In other words, a very minimalist web api is secured by adding an authorization layer before user requests can reach protected resources.  At this point it is expected that the user sign-in had already happened, so api calls can be made in the name of the signed-in user. For that to be possible a token containing user's information is being sent in the request headers and used in the authorization process.

<!-- TODO: IMAGE or CONSOLE OUTPUT of running/executed app -->

> :page_with_curl: This sample application backs one or more technical articles on docs.microsoft.com. <!-- TODO: Link to first tutorial in series when published. -->

## Prerequisites

- An Azure Active Directory (Azure AD) tenant. You can [open an Azure account for free](https://azure.microsoft.com/free) to get an Azure AD instance.
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

> :information_source: **Bold text** refers to a specific UI element in the Azure portal and `code formatting` indicates a value to enter or select.

### 2. Add scopes

Add the following scopes by using **Expose an API** in the web API's app registration:

| Scope name      | Who can consent?     | Admin consent display name | Admin consent description                               | User consent display name | User consent description                                | State                 |
|-----------------|----------------------|----------------------------|---------------------------------------------------------|---------------------------|---------------------------------------------------------|-----------------------|
| `Forecast.Read` | **Admins and users** | `Read forecast data`       | `Allows the application to read weather forecast data.` | `Read forecast data`      | `Allows the application to read weather forecast data.` | **Enabled** (default) |

### 3. Configure the code

In the _./appsettings.json_ file, replace these `{PLACEHOLDER}` values with the corresponding values from your web API's app registration:

```json
"ClientId": "{APPLICATION_CLIENT_ID}",
"TenantId": "{DIRECTORY_TENANT_ID}",
```

For example:

```json
"ClientId": "01234567-89ab-cdef-0123-4567890abcde",
"TenantId": "4567890a-bcde-f012-3456-789abcdef012",
```

## Run the application

### 1. Run the web API

Execute the following command to get the app up and running:

```bash
dotnet run
```

### 2. Send a request to the web API

To verify the endpoint is protected, use this cURL command to send an unauthenticated HTTP GET request to it:

```bash
# Execute unauthenticated request to protected API endpoint
curl -X GET https://localhost:5001/weatherforecast -ki
```

The expected response  is `401 Unauthenticated` because no access token was included in the request.

### 3. Clean up resources

If you no longer need the app registration, you can delete it by using the Azure portal or by running this Azure CLI command. # Replace `{APPLICATION_CLIENT_ID}` with the **Application (client) ID** of the app registration.

```bash
# Delete the web API's app registration
az ad app delete --id {APPLICATION_CLIENT_ID}
```

## About the code

The base project for this code sample was generated by using the ASP.NET Core minimal web API project template and installing the Microsoft.Identity.Web package from NuGet:

```bash
# Create ASP.NET minimal web API project
dotnet new webapi -minimal -o Api

# Add the Microsoft.Identity.Web assembly reference
dotnet add package Microsoft.Identity.Web
```

For more information about minimal web APIs in ASP.NET Core, see [Tutorial: Create a minimal web API with ASP.NET Core](https://learn.microsoft.com/aspnet/core/tutorials/min-web-api).

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
