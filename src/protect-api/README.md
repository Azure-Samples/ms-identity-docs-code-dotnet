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
# ASP.NET Core minimal web API | web api | access control (protected routes) | Microsoft identity platform

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

### 1. Register the web API application in your Azure Active Directory (Azure AD)

First, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the sample app.

Use the following settings for your app registration:

| App registration <br/> setting | Value for this sample app                                           | Notes                                                                                                       |
|:------------------------------:|:--------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------|
| **Name**                       | `active-directory-dotnet-minimal-api-aspnetcore`                    | Suggested value for this sample. <br/> You can change the app name at any time.                             |
| **Supported account types**    | **Accounts in this organizational directory only (Single tenant)**  | Required for this sample. <br/> Support for the Single tenant.                                              |
| **Platform type**              | **Web**                                                             | Required value for this sample. <br/> Enables the required and optional settings for the app type.          |
| **Identifier URI**             | `api://{clientId}`                                                  | Suggested value for this sample. <br/> You must change the client id using the Value shown in Azure portal. |
| **Scopes**                     | `Forecast.Read`                                                     | Required value for this sample.                                                                             |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Azure portal, while `code formatting` indicates a value you enter into a text box or select in the Azure portal.

<details>
  <summary>:computer: Alternative: register the application using az-cli</summary>

1. Register a new Azure AD app

   ```bash
   AZURE_AD_APP_CLIENT_ID_MINIMAL_API=$(az ad app create --display-name "active-directory-dotnet-minimal-api-aspnetcore" --query appId -o tsv)
   ```

1. Disable the default scope for `user_impersonation`

   ```bash
   AZURE_AD_APP_USER_IMPERSONATION_SCOPE=$(az ad app show --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API --query "oauth2Permissions[0]" -o json | sed 's#"isEnabled": true#"isEnabled": false#g') && \
   az ad app update --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API --set oauth2Permissions="[$AZURE_AD_APP_USER_IMPERSONATION_SCOPE]"
   ```

1. Create a new manifest scope for `forescast.read`

   ```bash
   cat > forescast.read.json <<EOF
   [
     {
       "adminConsentDescription": "Allows the app to access Minimal Api (active-directory-dotnet-minimal-api-aspnetcore) as the signed-in user.",
       "adminConsentDisplayName": "Access Minimal Api (active-directory-dotnet-minimal-api-aspnetcore)",
       "id": "1658e205-0e89-43a3-b107-b06a3e6dc60d",
       "isEnabled": true,
       "lang": null,
       "origin": "Application",
       "type": "User",
       "userConsentDescription": "Allow the application to access Minimal (active-directory-dotnet-minimal-aspnetcore) on your behalf.",
       "userConsentDisplayName": "Access Minimal Api (active-directory-dotnet-minimal-aspnetcore)",
       "value": "forescast.read"
     }
   ]
   EOF
   ```

1. Set a global unique URI that identify the web API and add the `forescast.read` scope

   ```bash
   az ad app update --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API --identifier-uris "api://${AZURE_AD_APP_CLIENT_ID_MINIMAL_API}" --set oauth2Permissions=@forescast.read.json
   ```
</details>

### 2. Configure the web API

1. Open the `Api.csproj` under the the `protect-api` folder in your code editor.
1. Open the `appsettings.json` file and modify the following code:

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
       "ClientId": "${AZURE_AD_APP_CLIENT_ID_MINIMAL_API}",
       "TenantId": "$(az account show --query tenantId --output tsv)",
       "Scopes": "forescast.read"
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
   curl -X GET https://localhost:5001/weatherforecast -ki
   ```

   :information_source: Since the request is sent without a Bearer Token, it is expected to receive an Unauthorized response `401`. The web API is now protected

### 3. Clean up

1. Delete the Azure AD app

   ```bash
   az ad app delete --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API
   ```

## About the code

The ASP.NET Core minimal web API application protects its weather endpoint, so it will only authorize calls in the name of signed-in users.

```output
┌──────────────────────────────────┐                               ┌─────────────────────────────┐
│                                  │                               │                             │
│   ASP.NET Core 6                 │        Http Request           │  ASP.NET Core 6             │
│                                  ├──────────────────────────────►│                             │
│   Web app that signs in users    │  Authorization Bearer 1NS...  │  Protected Minimal web Api  │
│                                  │  with forescast.read scope    │                             │
└──────────────────────────────────┘                               └─────────────────────────────┘

Scenario:

An (ASP.NET Core) web app that allows users to sign in enables the possibility of acquiring and validating their tokens for specific audiences and scopes.

Later the web app can make calls to protected Apis in the name of the signed-in users.
```

:link: For more information about how to protect your projects, please let's take a look at https://docs.microsoft.com/en-us/azure/active-directory/develop/sample-v2-code. To know more about how this sample has been generated, please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code

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
