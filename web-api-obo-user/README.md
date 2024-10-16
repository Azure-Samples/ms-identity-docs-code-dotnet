---
# Metadata required by https://docs.microsoft.com/samples/browse/
# Metadata properties: https://review.docs.microsoft.com/help/contribute/samples/process/onboarding?branch=main#add-metadata-to-readme
languages:
- csharp
page_type: sample
name: ASP.NET Core minimal web API that both protects its own endpoints and accesses Microsoft Graph.
description: This ASP.NET Core minimal web API protects an API endpoint that access on behalf of the user another protected API. The code in this sample is used by one or more articles on docs.microsoft.com.
products:
- azure
- entra-id
- ms-graph
urlFragment: ms-identity-docs-code-obo-user-csharp
---

# ASP.NET Core minimal web API | web api | access control (protected routes), protected web API access (Microsoft Graph) | Microsoft identity platform

<!-- Build badges here
![Build passing.](https://img.shields.io/badge/build-passing-brightgreen.svg) ![Code coverage.](https://img.shields.io/badge/coverage-100%25-brightgreen.svg) ![License.](https://img.shields.io/badge/license-MIT-green.svg)
-->

This ASP.NET Core minimal web API uses the Microsoft identity platform to protect an endpoint (require authorized access), and also accesses Microsoft Graph on behalf of the user. The API uses [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity?view=aspnetcore-8.0) interacting with the [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/azure/active-directory/develop/msal-overview) to protect its endpoint.

```console
$ curl https://localhost:5001/api/me -H "Authorization: Bearer {valid-access-token}"
{
  "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#users/$entity",
  "businessPhones": [],
  "displayName": "Maui",
  "givenName": null,
  "jobTitle": "dev",
  "mail": null,
  "mobilePhone": null,
  "officeLocation": null,
  "preferredLanguage": null,
  "surname": null,
  "id": "00aa11bb-cc22-dd33-ee44-ff55ee66dd77"
}
```

> :page_with_curl: This sample application backs one or more technical articles on docs.microsoft.com. <!-- TODO: Link to first tutorial in series when published. -->

## Prerequisites

- A Microsoft Entra tenant. You can [open an Azure account for free](https://azure.microsoft.com/free) to get a Microsoft Entra instance.
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup

### 1. Register the web API application in Microsoft Entra ID

First, complete the steps in [Configure an application to expose a web API](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the sample API and expose a scope.

Use the following settings for your app registration:

| App registration <br/> setting | Value for this sample app                                           | Notes                                                                                                       |
|------------------------------:|:--------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------|
| **Name**                       | `protected-api-access-protected-api`               | Suggested value for this sample. <br/> You can change the app name at any time.                             |
| **Supported account types**    | **Accounts in this organizational directory only (Single tenant)**  | Required for this sample. <br/> Support for the Single tenant.                                              |
| **Identifier URI**             | `api://{clientId}`                                                  | Suggested value for this sample. <br/> You must change the client id using the Value shown in the Microsoft Entra admin center. |
| **Expose an API**              | **Scope name**: `user_impersonation`<br/>**Who can consent?**: **Admins and users**<br/>**Admin consent display name**: `Act on behalf of the user`<br/>**Admin consent description**: `Allows the API to act on behalf of the user.`<br/>**User consent display name**: `Act on your behalf`<br/>**User consent description**: `Allows the API to act on your behalf.`<br/>**State**: **Enabled**  | Add a new scope that reads as follows `api://{clientId}/user_impersonation`. Required value for this sample. |
| **API Permissions**            | `https://graph.microsoft.com/User.Read`                             | Add a new delegated permission for `Microsoft Graph User.Read`. Required value for this sample.   |
| **Client secret**              | _Value shown in Microsoft Entra admin center_                                       | :warning: Record this value immediately! <br/> It's shown only _once_ (when you create it).                 |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box or select in the Microsoft Entra admin center.

<a name='2-register-a-client-application-in-azure-ad'></a>

### 2. Register a client application in Microsoft Entra ID

Second, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the client sample app.

Use the following settings for your app registration:

| App registration <br/> setting | Value for this sample app                                           | Notes                                                                                                       |
|------------------------------:|:--------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------|
| **Name**                       | `active-directory-curl-app`                                         | Suggested value for this sample. <br/> You can change the app name at any time.                             |
| **Supported account types**    | **Accounts in this organizational directory only (Single tenant)**  | Required for this sample. <br/> Support for the Single tenant.                                              |
| **Platform type**              | **Web**                                                             | Required value for this sample. <br/> Enables the required and optional settings for the app type.          |
| **API Permissions**            | `api://{clientId}/user_impersonation`                               | Add a delegated type permission by searching within the APIs using the new Application (client) ID from the previous step. Then select the `user_impersonation`. Required value for this sample. |
| **Client secret**              | _Value shown in Microsoft Entra admin center_                                       | :warning: Record this value immediately! <br/> It's shown only _once_ (when you create it).                 |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box or select in the Microsoft Entra admin center.

### 3. Go back to the recently registered web API application

Third, modify the web API application to update the following settings to reference the cUrl app

| App registration <br/> setting    | Value for this sample app                                        | Notes                                                                                              |
|---------------------------------:|:-----------------------------------------------------------------|:---------------------------------------------------------------------------------------------------|
| **knownClientApplications**       | Client ID (UUID) of the application created in step 2.           | Required value for this sample.                                                                    |

### 4. Configure the web API

1. Open the _protected-api-access-protected-api/appsettings.json_ file in your code editor and modify the following code:

   ```json
   "ClientId": "Enter_the_Application_Id_here",
   "TenantId": "Enter_the_Tenant_Info_here",
   "ClientSecret": "Enter_the_Client_Secret_here"
   ```

## Run the application

### 1. Run the web API

Execute the following command to get the app up and running:

```bash
dotnet run
```

### 2. Send request to the web API

1. Once the web API is listening, execute the following to send a request to its protected endpoint.

   ```bash
   curl -X GET https://localhost:5001/api/me -ki
   ```

   :information_source: The expected response is `401 Unauthorized` because you've sent a request to the protected endpoint without including an access token (as a bearer token).

1. Use Postman, curl, or similar to send an HTTP GET request to *https:\/\/localhost:5001\/me*, this time including an `Authorization` header of `Bearer {valid-access-token}`.

    If everything worked, your protected web API should return a response similar to this:

   ```console
   curl -X GET https://localhost:5001/api/me -ki -H "Authorization: Bearer {valid-access-token}"
   {
    "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#users/$entity",
    "businessPhones": [],
    "displayName": "Maui",
    "givenName": null,
    "jobTitle": "dev",
    "mail": null,
    "mobilePhone": null,
    "officeLocation": null,
    "preferredLanguage": null,
    "surname": null,
    "id": "cff40dac-17ea-4183-9caf-65f2ee90c562"
   }
   ```

   :information_source: The expected response code is `200 OK` if you included a valid access token as bearer token in the request.

## About the code

This ASP.NET Core minimal web API has a single protected route, (_/api/me_), that requires callers (client applications making requests to the endpoint) present a valid access token issued by the Microsoft identity platform.

Acting as a middle-tier API, this minimal web API then uses that access token to acquire a second access token from the Microsoft identity platform, this time for Microsoft Graph, on behalf of the user.

Finally, the web API requests data from the Microsoft Graph `/me` endpoint and includes the response data in its response to the original caller.

This project uses the **Microsoft.Identity.Web** apis to interface with [Microsoft Authentication Library (MSAL) for .NET](https://github.com/azuread/microsoft-authentication-library-for-dotnet). It acquires access tokens on behalf of the user, caching the resulting token in memory. Provided an access token was previously cached, the subsequent calls against _/api/me_ will attempt to reuse the cached access token, refreshing it if nearing expiration. The MSAL is logging informational entries that state when a new access token is being acquired, cached, and re-used.

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
