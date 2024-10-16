---
# Metadata required by https://docs.microsoft.com/samples/browse/
# Metadata properties: https://review.docs.microsoft.com/help/contribute/samples/process/onboarding?branch=main#add-metadata-to-readme
languages:
- csharp
page_type: sample
name: .NET (C#) console application that makes a request to the Graph API via the Device Code flow
description: This .NET8 console application uses the device code flow for authentication and then makes a request to Microsoft Graph for the user's profile data.
products:
- azure
- entra-id
- ms-graph
urlFragment: ms-identity-docs-code-console-cli-csharp
---

# .NET (C#) | console | user sign-in, protected web API access (Microsoft Graph) | Microsoft identity platform

This .NET 8 (C#) console application authenticates a user via the device code flow, and then makes a request to the Graph API as the authenticated user. The response to the request is printed to the console.

```console
$ dotnet run
To sign in, use a web browser to open the page https://microsoft.com/devicelogin and enter the code XXXXXXXXX to authenticate.
{
  "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#users/$entity",
  "businessPhones": ["+1 (000) 1112222"],
  "displayName": "Contoso Employee",
  "givenName": "Contoso",
  "jobTitle": "Worker",
  "mail": "cemployee@contoso.com",
  "mobilePhone": "1 000-111-2222",
  "officeLocation": "Contoso Plaza/F30",
  "preferredLanguage": null,
  "surname": "Employee",
  "userPrincipalName": "contoso_employee@contoso.com",
  "id": "00aa11bb-cc22-dd33-ee44-ff55ee66dd77"
}
```

## Prerequisites

- Microsoft Entra tenant and the permissions or role required for managing app registrations in the tenant.
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup

### 1. Register the app

First, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the application.

Use these settings in your app registration.

| App registration <br/> setting   | Value for this sample app                                          | Notes                                                                           |
|---------------------------------:|:-------------------------------------------------------------------|:--------------------------------------------------------------------------------|
| **Name**                         | `dotnet-device-code-flow-app`                                      | Suggested value for this sample. <br/> You can change the app name at any time. |
| **Supported account types**      | **Accounts in this organizational directory only (Single tenant)** | Suggested value for this sample.                                                |
| **Platform type**                | _None_                                                             | No redirect URI required; don't select a platform.                              |
| **Allow public client flows**    | **Yes**                                                            | Required value for this sample.                                                 |

> :information_source: **Bold text** in the tables above matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box in the Microsoft Entra admin center.

### 2. Update the _Program.cs_ file with app registration values

```csharp
// 'Directory (tenant) ID' of app registration in the Microsoft Entra admin center - this value is a GUID
TenantId = "",

// 'Application (client) ID' of app registration in Microsoft Entra admin center - this value is a GUID
ClientId = ""
```

## Run the application

```bash
dotnet run
```

Follow the device code flow instructions that are presented. If everything worked, you should receive a response similar to this:

```json
{
  "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#users/$entity",
  "businessPhones": ["+1 (000) 1112222"],
  "displayName": "Contoso Employee",
  "givenName": "Contoso",
  "jobTitle": "Worker",
  "mail": "cemployee@contoso.com",
  "mobilePhone": "1 000-111-2222",
  "officeLocation": "Contoso Plaza/F30",
  "preferredLanguage": null,
  "surname": "Employee",
  "userPrincipalName": "contoso_employee@contoso.com",
  "id": "00aa11bb-cc22-dd33-ee44-ff55ee66dd77"
}
```

## About the code

This .NET 6 (C#) console application prompts the user to sign in via their device using a code provided by Microsoft Authentication Library (MSAL). The user completes this flow in their chosen web browser. Upon successful authentication, an HTTP GET request to the Microsoft Graph /me endpoint is issued with the user's access token in the HTTP header. The response from the GET request is then displayed in the console.

This sample does not demonstrate the usage of cached access tokens. Access token caching should be used in situations where the console application will need to access the same protected API using the same access token multiple times across the life of the user's session in the application. This sample, as written, does not perform multiple calls and wouldn't result in a token cache hit. For additional information, see [Get a token from the token cache using MSAL.NET](https://docs.microsoft.com/azure/active-directory/develop/msal-net-acquire-token-silently).

## Reporting problems

### Sample app not working?

If you can't get the sample working, you've checked [Stack Overflow](http://stackoverflow.com/questions/tagged/msal), and you've already searched the issues in this sample's repository, open an issue report the problem.

1. Search the [GitHub issues](../../issues) in the repository - your problem might already have been reported or have an answer.
1. Nothing similar? [Open an issue](../../issues/new) that clearly explains the problem you're having running the sample app.

### All other issues

> :warning: WARNING: Any issue in this repository _not_ limited to running one of its sample apps will be closed without being addressed.

For all other requests, see [Support and help options for developers | Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/developer-support-help-options).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
