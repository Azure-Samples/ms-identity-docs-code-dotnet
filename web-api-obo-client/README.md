---
# Metadata required by https://docs.microsoft.com/samples/browse/
# Metadata properties: https://review.docs.microsoft.com/help/contribute/samples/process/onboarding?branch=main#add-metadata-to-readme
languages:
- csharp
page_type: sample
name: "ASP.NET Core minimal web API that makes a request to the Graph API as itself"
description: "This ASP.NET Core minimal web API sample demonstrates how to issue a call to a protected API using the client credentials flow.  A request will be issued to Microsoft Graph using the application's own identity."
products:
- azure
- entra-id
- ms-graph
urlFragment: ms-identity-docs-code-app-csharp-webapi
---

# ASP.NET Core minimal web API | Web API | Web API that accesses a protected web API (Microsoft Graph) | Microsoft identity platform

<!-- Build badges here
![Build passing.](https://img.shields.io/badge/build-passing-brightgreen.svg) ![Code coverage.](https://img.shields.io/badge/coverage-100%25-brightgreen.svg) ![License.](https://img.shields.io/badge/license-MIT-green.svg)
-->

This ASP.NET Core minimal web API issues a call to a protected web API (Microsoft Graph) by using the OAuth 2.0 client credentials flow. The request to the Microsoft Graph endpoint is issued using the ASP.NET Core minimal web API's own identity.

```console
$ curl https://localhost:5001/api/application
{
   "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#applications/$entity",
   "id": "00aa11bb-cc22-dd33-ee44-ff55ee66dd77",
   "deletedDateTime": null,
   "appId": "00001111-aaaa-2222-bbbb-3333cccc4444",
   "applicationTemplateId": null,
   "disabledByMicrosoftStatus": null,
   "createdDateTime": "2022-02-23T21:35:20Z",
   "displayName": "active-directory-dotnet-minimal-api-aspnetcore-client-credentail-flow",
   "description": null,
   "groupMembershipClaims": null,
   "identifierUris": [],
   "isDeviceOnlyAuthSupported": null,
   "isFallbackPublicClient": null,
   "notes": null,
   "publisherDomain": "contoso.onmicrosoft.com",
   "serviceManagementReference": null,
   "signInAudience": "AzureADMyOrg",
   "tags": [],
   "tokenEncryptionKeyId": null,
   "defaultRedirectUri": null,
   "certification": null,
   "optionalClaims": null,
   ...
}
```

## Prerequisites

- A Microsoft Entra tenant. You can [open an Azure account for free](https://azure.microsoft.com/free) to get a Microsoft Entra instance.
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup

### 1. Register the app

First, complete the steps in [Quickstart: Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the web API.

Use these settings in your app registration.

| App registration <br/> setting    | Value for this sample app                                                    | Notes                                                                                              |
|---------------------------------:|:------------------------------------------------------------------------------|:---------------------------------------------------------------------------------------------------|
| **Name**                          | `active-directory-dotnet-minimal-api-aspnetcore-client-credentail-flow`      | Suggested value for this sample. <br/> You can change the app name at any time.                    |
| **Supported account types**       | **Accounts in this organizational directory only (Single tenant)**           | Suggested value for this sample.                                                                   |
| **Platform type**                 | _None_                                                                       | No redirect URI required; don't select a platform.                                                                    |
| **Client secret**                 | _**Value** of the client secret (not its ID)_                                | :warning: Record this value immediately! <br/> It's shown only _once_ (when you create it).        |

> :information_source: **Bold text** in the tables above matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box in the Microsoft Entra admin center.

### 2. Configure the web API

Open the _~/msal-client-credentials-flow/appsettings.json_ file in your code editor and modify the following values values with those from your [app's registration in the Microsoft Entra admin center](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app#register-an-application):

   ```json
   "ClientId": "Enter_the_Application_Id_here",
   "TenantId": "Enter_the_Tenant_Info_Here",
   "ClientSecret": "Enter_the_Application_CLient_Secret_Here"
   ...
   "RelativePath": "Enter_the_Application_Object_Id_Here",
   ```

## Run the application

### 1. Run the web API

Execute the following command to get the app up and running:

   ```bash
   dotnet run
   ```

### 2. Send request to the web API

Once the app is running and listening for requests, execute the following command to send it a request.


```bash
curl -X GET https://localhost:5001/api/application -ki
```

If everything worked, you should receive a response from the downstream web API (Microsoft Graph, in this case) similar to this:

```console
$ curl https://localhost:5001/api/application -ki
{
   "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#applications/$entity",
   "id": "00aa11bb-cc22-dd33-ee44-ff55ee66dd77",
   "deletedDateTime": null,
   "appId": "00001111-aaaa-2222-bbbb-3333cccc4444",
   "applicationTemplateId": null,
   "disabledByMicrosoftStatus": null,
   "createdDateTime": "2022-02-23T21:35:20Z",
   "displayName": "active-directory-dotnet-minimal-api-aspnetcore-client-credentail-flow",
   "description": null,
   "groupMembershipClaims": null,
   "identifierUris": [],
   "isDeviceOnlyAuthSupported": null,
   "isFallbackPublicClient": null,
   "notes": null,
   "publisherDomain": "contoso.onmicrosoft.com",
   "serviceManagementReference": null,
   "signInAudience": "AzureADMyOrg",
   "tags": [],
   "tokenEncryptionKeyId": null,
   "defaultRedirectUri": null,
   "certification": null,
   "optionalClaims": null,
   ...
}
```

## About the code

This ASP.NET Core minimal web API has a single route (_/api/application_) that supports anonymous access.  When a client app calls the anonymous route on this API, the API requests its own application object from Microsoft Graph and then returns that data to the client.

This web API uses [Microsoft Authentication Library (MSAL)](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet).

This project is configured to acquire an access token using the client credential flow, caching the resulting token in memory. Provided an access token was previously cached, the subsequent calls against _/api/application_ will attempt to reuse the cached access token, refreshing it if nearing expiration. The MSAL is logging informational entries that state when a new access token is being acquired, cached, and re-used.

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
