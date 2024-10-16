---
# Metadata required by https://docs.microsoft.com/samples/browse/
# Metadata properties: https://review.docs.microsoft.com/help/contribute/samples/process/onboarding?branch=main#add-metadata-to-readme
languages:
- csharp
page_type: sample
name: .NET console application that accesses a protected API
description: This a .NET console application that accesses a protected API. The code in this sample is used by one or more articles on docs.microsoft.com.
products:
- azure
- azure-active-directory
- ms-graph
urlFragment: ms-identity-docs-code-dotnet-console
---

# .NET | console | .NET (C#) console app that accesses a protected web API access (Microsoft Graph) | Microsoft identity platform


This .NET console application accesses protected web API (Microsoft Graph) as its own identity by using the [Microsoft Authentication Library (MSAL) for .NET](https://learn.microsoft.com/en-us/entra/msal/dotnet/). The application is written in C# and supports scenarios like cron jobs and direct command-line invocation.

```bash
dotnet run

Could not find a cached token, so fetching a new one.
{
  "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#applications/$entity",
  "id": "00aa11bb-cc22-dd33-ee44-ff55ee66dd77",
  "deletedDateTime": null,
  "appId": "00001111-aaaa-2222-bbbb-3333cccc4444",
  "applicationTemplateId": null,
  "disabledByMicrosoftStatus": null,
  "createdDateTime": "2021-01-17T15:30:55Z",
  "displayName": "identity-dotnet-console-app",
  "description": null,
  "groupMembershipClaims": null,
  ...
}
```

## Prerequisites

- An Azure account with an active subscription. If you don't already have one, [Create an account for free](https://azure.microsoft.com/free/?WT.mc_id=A261C142F).
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup

### 1. Register the app with the Microsoft identity platform

First, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the application.

Use these settings in your app registration.

| App registration <br/> setting   | Value for this sample app       | Notes                                                                                  |
|-----------------------------:|:------------------------------------|:---------------------------------------------------------------------------------------|
| **Name**                     | `identity-client-daemon-app`       | Suggested value for this sample. <br/> You can change the app name at any time.        |
| **Supported account types**  | **Accounts in this organizational directory only (Single tenant)** | Suggested value for this sample.                        |
| **Platform type**            | _None_                                                             | No redirect URI required; don't select a platform.      |
| **Client secret**            | _**Value** of the client secret (not its ID)_                      | :warning: Record this value immediately! <br/> It's shown only _once_ (when you create it). |

> :information_source: **Bold text** in the tables above matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box in the Microsoft Entra admin center.

### 2. Update application code with values from app registration

In _Program.cs_, update each variable with values from the app registration you created in the previous step.

```csharp
// Full directory URL, in the form of https://login.microsoftonline.com/<tenant_id>
Authority = " https://login.microsoftonline.com/Enter the tenant ID obtained from the Microsoft Entra admin center",
// 'Enter the client ID obtained from the Microsoft Entra admin center
ClientId = "Enter the client ID obtained from the Microsoft Entra admin center",
// Client secret 'Value' (not its ID) from 'Client secrets' in the Microsoft Entra admin center
ClientSecret = "Enter the client secret value obtained from the Microsoft Entra admin center",
// Client 'Object ID' of app registration in Microsoft Entra admin center - this value is a GUID
ClientObjectId = "Enter the client Object ID obtained from the Microsoft Entra admin center"
```

## Run the application

```bash
dotnet run
```

If successful, an output simialir to the following is displayed in the console (response shortened for brevity):

```json
{
  "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#applications/$entity",
  "id": "00aa11bb-cc22-dd33-ee44-ff55ee66dd77",
  "deletedDateTime": null,
  "appId": "00001111-aaaa-2222-bbbb-3333cccc4444",
  "applicationTemplateId": null,
  "disabledByMicrosoftStatus": null,
  "createdDateTime": "2021-01-17T15:30:55Z",
  "displayName": "identity-dotnet-console-app",
  "description": null,
  "groupMembershipClaims": null,
  ...
}
```

## About the code

This .NET (C#) console application uses a client secret as its credentials to retrieve an access token that's scoped for the Microsoft Graph API, and then uses that token to access its own application registration information.

The Microsoft Graph response data is then written to the console. This .NET (C#) console app uses [Microsoft Authentication Library (MSAL)](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet).

## Reporting problems

### Sample app not working?

If you can't get the sample working, you've checked [Stack Overflow](http://stackoverflow.com/questions/tagged/msal), and you've already searched the issues in this sample's repository, open an issue report the problem.

1. Search the [GitHub issues](../../issues) in the repository - your problem might already have been reported or have an answer.
1. Nothing similar? [Open an issue](../../issues/new) that clearly explains the problem you're having running the sample app.

### All other issues

> :warning: WARNING: Any issue in this repository _not_ limited to running one of its sample apps will be closed without being addressed.
For all other requests, see [Support and help options for developers | Microsoft identity platform](https://learn.microsoft.com/entra/identity-platform/developer-support-help-options).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
