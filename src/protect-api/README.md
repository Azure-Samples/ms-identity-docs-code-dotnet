---
# Metadata required by https://docs.microsoft.com/samples/browse/
# Metadata properties: https://review.docs.microsoft.com/help/contribute/samples/process/onboarding?branch=main#add-metadata-to-readme
languages:
- csharp
page_type: sample
name: ASP.NET Core minimal web API that protects API
description: "This ASP.NET Core minimal web API protects an API endpoint. The code in this sample is used by one or more articles on docs.microsoft.com."
products:
- azure
- azure-active-directory
- ms-graph
urlFragment: ms-identity-docs-code-csharp
---
# ASP.NET Core minimal web API - Protects API | Microsoft identity platform

The sample code provided here has been created using minimal web API in ASP.NET Core 6.0, and slightly modified to be protected for a single organization using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) that interacts with [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview).  In other words, a very minimalist web api is secured by adding an authorization layer before user requests can reach protected resources.  At this point it is expected that the user sign-in had already happened, so api calls can be made in the name of the signed-in user. For that to be possible a token containing user's information is being sent in the request headers and used in the authorization process.

> :link: For more information about how to protect your projects, please let's take a look at https://docs.microsoft.com/en-us/azure/active-directory/develop/sample-v2-code. To know more about how this sample has been generated, please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code

## Prerequisites

1. [Download .NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Setup

### 1. Register the web API application in your Azure Active Directory (Azure AD)

First, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the sample app.

Use the following settings for your app registration:

- Name: `active-directory-dotnet-minimal-api-aspnetcore` (suggested)
- Supported account types: **Accounts in any organizational directory and personal Microsoft accounts**
- Platform type: `web`
- Indentifier URIs: `api://{AZURE_AD_APP_CLIENT_ID_MINIMAL_API}`
- Scopes: `Forecast.Read`

### 2. Configure the web API

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

   :book: Since the request is sent without a Bearer Token, it is expected to receive an Unauthorized response `401`. The web API is now protected

### 3. Clean up

1. Delete the Azure AD app

   ```bash
   az ad app delete --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API
   ```

## About the code

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

## Reporting problems

### Sample app not working?

If you can't get the sample working, you've checked [Stack Overflow](http://stackoverflow.com/questions/tagged/msal), and you've already searched the issues in this sample's repository, open an issue report the problem.

1. Search the [GitHub issues](../../../../issues) in the repository - your problem might already have been reported or have an answer.
1. Nothing similar? [Open an issue](LINK_HERE) that clearly explains the problem you're having running the sample app.

### All other issues

> :warning: WARNING: Any issue _not_ limited to running this or another sample app will be closed without being addressed.

For all other requests, see [Support and help options for developers | Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/developer-support-help-options).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
