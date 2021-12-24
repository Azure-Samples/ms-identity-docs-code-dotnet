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
- azure-active-directory
- ms-graph
urlFragment: ms-identity-docs-code-csharp
---
# ASP.NET Core minimal web API | web api | access control (protected routes), protected web API access (Microsoft Graph) | Microsoft identity platform

<!-- Build badges here
![Build passing.](https://img.shields.io/badge/build-passing-brightgreen.svg) ![Code coverage.](https://img.shields.io/badge/coverage-100%25-brightgreen.svg) ![License.](https://img.shields.io/badge/license-MIT-green.svg)
-->

This sample demonstrates an ASP.NET Core minimal web API  that is both protected by Microsoft identity platform and accesses Microsoft Graph on behalf of the user by using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) that interacts with [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview).

<!-- TODO: IMAGE or CONSOLE OUTPUT of running/executed app -->

> :page_with_curl: This sample application backs one or more technical articles on docs.microsoft.com. <!-- TODO: Link to first tutorial in series when published. -->

## Prerequisites

- An Azure Active Directory (Azure AD) tenant. You can [open an Azure account for free](https://azure.microsoft.com/free) to get an Azure AD instance.
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Setup

### 1. Register the web API application in your Azure Active Directory (Azure AD)

First, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the sample app.

Use the following settings for your app registration:

| App registration <br/> setting | Value for this sample app                                           | Notes                                                                                                       |
|:------------------------------:|:--------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------|
| **Name**                       | `active-directory-protected-api-access-protected-api`               | Suggested value for this sample. <br/> You can change the app name at any time.                             |
| **Supported account types**    | **Accounts in this organizational directory only (Single tenant)**  | Required for this sample. <br/> Support for the Single tenant.                                              |
| **Platform type**              | **Web**                                                             | Required value for this sample. <br/> Enables the required and optional settings for the app type.          |
| **Identifier URI**             | `api://{clientId}`                                                  | Suggested value for this sample. <br/> You must change the client id using the Value shown in Azure portal. |
| **Expose an API**              | `api://{clientId}/user_impersonation`                               | Create a new delegated permission called user_impersonation. Required value for this sample.                |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Azure portal, while `code formatting` indicates a value you enter into a text box or select in the Azure portal.

### 2. Register a client application in your Azure Active Directory (Azure AD)

Second, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the client sample app.

Use the following settings for your app registration:

| App registration <br/> setting | Value for this sample app                                           | Notes                                                                                                       |
|:------------------------------:|:--------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------|
| **Name**                       | `active-directory-curl-app`                                         | Suggested value for this sample. <br/> You can change the app name at any time.                             |
| **Supported account types**    | **Accounts in this organizational directory only (Single tenant)**  | Required for this sample. <br/> Support for the Single tenant.                                              |
| **Platform type**              | **Web**                                                             | Required value for this sample. <br/> Enables the required and optional settings for the app type.          |
| **API Permissions**            | `api://{clientId}/user_impersonation`                               | Create a new delegated permission called user_impersonation. Required value for this sample.                |
| **Client secret**              | _Value shown in Azure portal_                                       | :warning: Record this value immediately! <br/> It's shown only _once_ (when you create it).                 |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Azure portal, while `code formatting` indicates a value you enter into a text box or select in the Azure portal.

### 3. Go back to the recently registered web API application

Third, modify the web API application to update the following settings to reference the cUrl app

| App registration <br/> setting    | Value for this sample app                                        | Notes                                                                                              |
|:---------------------------------:|:-----------------------------------------------------------------|:---------------------------------------------------------------------------------------------------|
| **knownClientApplications**       | Client ID (UUID) of the application created in step 2.           | Required value for this sample.                                                                    |

### 4. Configure the web API

1. Open the _~/protected-api-access-protected-api/appsettings.json_ file in your code editor and modify the following code:

   ```json
   "ClientId": "Enter_the_Application_Id_here",
   "TenantId": "Enter_the_Tenant_Info_Here"
   ```

<details>
  <summary>:computer: Alternative: modify the appsettings.json file from your terminal</summary>

1. Create the `appsettings.json` file with the Azure AD app configuration

   ```bash
   cat > appsettings.json <<EOF
   {
     "AzureAd": {
       "Instance": "https://login.microsoftonline.com/",
       "ClientId": "${AZURE_AD_APP_CLIENT_ID_PROTECTED_API}",
       "TenantId": "$(az account show --query tenantId --output tsv)"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   EOF
   ```
</details>

## Run the application

### 1. Run the web API

1. Execute the following command to get the app up and running:

   ```bash
   dotnet run
   ```

### 2. Send request to the web API

1. Once the app is listening, execute the following to send the a request.

   ```bash
   curl -X GET https://localhost:5001/me -ki
   ```

   :information_source: Since the request is sent without a Bearer Token, it is expected to receive an Unauthorized response `401`. The web API is now protected

1. Set a shell environment variable containing a client secret for the app (for the `--password` argument in the next step):

    ```bash
    AZURE_AD_CURL_APP_SECRET=<at-least-sixteen-characters-here>
    ```

1. Register the cUrl app in Azure AD. This will act as a user.

   ```bash
   AZURE_AD_APP_CLIENT_ID_CURL_APP=$(az ad app create --display-name "active-directory-curl-app" --password ${AZURE_AD_CURL_APP_SECRET} --query appId -o tsv)
   ```

1. Acquire an Azure Security Access Token

   ```bash
   # TODO
   ```

1. execute the following to send the another request. This time, it sends Authorization Beare {token} to gain access

   ```bash
   curl -X GET https://localhost:5001/me -ki
   ```

   :information_source: Since the request is sent with a Bearer Token, it is expected to receive an `OK` response `200`. The web API was already protected and now it demostrate it can access another protected API on behalf of the the user.

### 3. Clean up

1. Delete the Azure AD web api app

   ```bash
   az ad app delete --id $AZURE_AD_APP_CLIENT_ID_PROTECTED_API
   ```

1. Delete the Azure AD cUrl app

   ```bash
   az ad app delete --id $AZURE_AD_APP_CLIENT_ID_CURL_APP
   ```

## About the code

This ASP.NET Core application uses minimal web API. The app has a single route requiring a valid Security Token that is going to be used to acquire another token on behalf of the user to access the `/me` protected Microsoft Graph endpoint.

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
