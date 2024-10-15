---
# Metadata required by https://docs.microsoft.com/samples/browse/
# Metadata properties: https://review.docs.microsoft.com/help/contribute/samples/process/onboarding?branch=main#add-metadata-to-readme
languages:
- csharp
page_type: sample
name: C# Azure Function that protects an HTTP trigger function with Easy Auth and access token scope validation.
description: This C# Azure Function protects its own HTTP Trigger function with Easy Auth and access token scope validation. The code in this sample is used by one or more articles on docs.microsoft.com.
products:
- azure
- entra-id
- azure-functions
urlFragment: ms-identity-docs-code-azure-function-csharp
---

# .NET (C#) | Azure function app | Protected web API | Microsoft identity platform

<!-- Build badges here
![Build passing.](https://img.shields.io/badge/build-passing-brightgreen.svg) ![Code coverage.](https://img.shields.io/badge/coverage-100%25-brightgreen.svg) ![License.](https://img.shields.io/badge/license-MIT-green.svg)
-->

This Azure Function written in C# protects its own API (HTTP trigger) with the combination of the built-in authentication and authorization feature of Azure Functions and Azure App Service (sometimes called _Easy Auth_) and JWT access token scope validation.

```console
$ curl https://<your-function>.azurewebsites.net/api/greeting -H "Authorization: Bearer <valid-access-token>"
Hello, world. You were able to access this because you provided a valid access token with the Greeting.Read scope as a claim.
```

> :page_with_curl: This sample application backs one or more technical articles on docs.microsoft.com. <!-- TODO: Link to first tutorial in series when published. -->

## Prerequisites

- Microsoft Entra tenant and the permissions or role required for managing app registrations in the tenant.
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- An empty [C# Azure function (v4)](https://docs.microsoft.com/azure/azure-functions/create-first-function-cli-csharp) deployed to Azure, and the permissions or role required to modify its settings.
- [Azure Functions Core Tools](https://docs.microsoft.com/azure/azure-functions/functions-run-local)

## Setup

### 1. Register the app

First, complete the steps in [Register an API application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-configure-app-expose-web-apis#register-the-web-api) to register the sample app.

Use these settings in your app registration.

| App registration <br/> setting | Value for this sample app                                            | Notes                                                                           |
|-------------------------------:|:---------------------------------------------------------------------|:--------------------------------------------------------------------------------|
| **Name**                       | `csharp-azure-function-api`                                          | Suggested value for this sample. <br/> You can change the app name at any time. |
| **Supported account types**    | **Accounts in this organizational directory only (Single tenant)**   | Suggested value for this sample.                                                |
| **Platform type**              | _None_                                                               | No redirect URI required; don't select a platform.                              |
| **Scopes defined by this API** | **Scope name**: `Greeting.Read`<br/>**Who can consent?**: **Admins and users**<br/>**Admin consent display name**: `Read API Greetings`<br/>**Admin consent description**: `Allows the user to see greetings from the API.`<br/>**User consent display name**: `Read API Greetings`<br/>**User consent description**: `Allows you to see greetings from the API.`<br/>**State**: **Enabled** | Required scope for this sample. |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box in the Microsoft Entra admin center.

### 2. Enable Function app authentication

Next, complete the steps in [Enable Microsoft Entra ID in your App Service app](https://docs.microsoft.com/azure/app-service/configure-authentication-provider-aad?toc=/azure/azure-functions/toc.json#-enable-azure-active-directory-in-your-app-service-app) to add Microsoft Entra ID as an identity provider for your API.

Use these settings in your identity provider configuration.

| Identity provider setting       | Value for this sample app                               | Notes                                                                            |
|--------------------------------:|:--------------------------------------------------------|:---------------------------------------------------------------------------------|
| **Identity provider**           | **Microsoft**                                           | Required value for this sample.                                                  |
| **App registration type**       | **Provide the details of an existing app registration** | Required value for this sample.                                                  |
| **Application (client) ID**     | `<client-id>`                                           | Required value for this sample. <br/> 'Application (client) ID' of the API's app registration in Microsoft Entra admin center - this value is a GUID     |
| **Client secret (recommended)** | _None_                                                  | Suggested value for this sample. <br/> This sample doesn't require this feature. |
| **Issuer URL**                  | `https://login.microsoftonline.com/<tenant-id>/v2.0`    | Required value for this sample. <br/> Update to include 'Tenant ID' of your Microsoft Entra instance - this value is a GUID                     |
| **Allowed token audiences**     | `api://<client-id>`                                     | Required value for this sample. <br/> 'Application ID URI' of app registration in Microsoft Entra admin center - this value typically starts with api:// |
| **Restrict access**             | **Require authentication**                              | Required value for this sample.                                                  |
| **Unauthenticated requests**    | **HTTP 401 Unauthorized: recommended for APIs**         | Suggested value for this sample.                                                 |
| **Token store**                 | _Unselected_                                            | Suggested value for this sample.                                                 |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box in the Microsoft Entra admin center.

### 3. Deploy the Functions app

Use the Azure Functions Core Tools CLI to deploy the function app to Azure:

```console
func azure functionapp publish <your-function-app-name>
```

## Access the API

Using Postman, curl, or a similar application, issue an HTTP GET request to https://\<your-function-app-name>.azurewebsites.net/api/greeting with an `Authorization` header of `Bearer {VALID-ACCESS-TOKEN}`.

For example, if you use curl and everything worked, you should receive a response from the Azure Function similar to this.

```console
$ curl https://<your-function>.azurewebsites.net/api/greeting -H "Authorization: Bearer <VALID-ACCESS-TOKEN>"
Hello, world. You were able to access this because you provided a valid access token with the Greeting.Read scope as a claim.
```

## About the code

This Azure Function is an anonymous HTTP trigger written in csharp and uses the built-in [Authentication and authorization in Azure Functions](https://docs.microsoft.com/azure/app-service/overview-authentication-authorization) feature to offload fundamental JWT access token validation. Requests that make it through the built-in authentication feature of Azure Functions are then routed to the csharp code, which applies additional access token validation checking for a specific scope.

- A missing or invalid (expired, wrong audience, etc) token will result in a `401` response. (Handled by Azure Functions authentication)
- An otherwise valid token without the proper scope will result in a `403` response.
- A valid token with the proper scope of `Greeting.Read` will result in the "Hello, world" message.

### Running locally

At the time of this writing, Function app authentication does not support a local development experience that has parity with the on-Azure runtime. You can still execute this locally with `func start` but the authentication functionality provided by the Function app service on Azure will not be invoked; all JWT token validation for authorization (signature, iss, exp, aud) will be skipped.

## Reporting problems

### Sample app not working?

If you can't get the sample working, you've checked [Stack Overflow](https://stackoverflow.com/search?q=%22easy+auth%22), and you've already searched the issues in this sample's repository, open an issue report the problem.

1. Search the [GitHub issues](/issues) in the repository - your problem might already have been reported or have an answer.
1. Nothing similar? [Open an issue](/issues/new) that clearly explains the problem you're having running the sample app.

### All other issues

> :warning: WARNING: Any issue in this repository _not_ limited to running one of its sample apps will be closed without being addressed.

For all other requests, see [Support and help options for developers | Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/developer-support-help-options).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
