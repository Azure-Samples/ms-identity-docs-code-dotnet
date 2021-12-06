# ASP.NET Core 6.0 Web App - Sign-in user | Microsoft identity platform

The web app in this scenario has been created using the ASP.NET Core 6.0 Razor template, and slightly modified to add authentication enabling the users sign-in that follows the [Open Id Connect](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc) standard protocol. To lite up Open Id, it is using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) middlewares.  In other words, a simple web app is secured by adding an authentication layer allowing users to sign-in with their Work and school (Azure AD) accounts, and as a result it can make web API calls to protected resources on behalf of the signed-in user.

> :page_with_curl: This sample application backs one or more technical articles on docs.microsoft.com. <!-- TODO: Link to first tutorial in series when published. -->

## Prerequisites

1. An Azure Active Directory (Azure AD) tenant. You can [open an Azure account for free](https://azure.microsoft.com/free) to get an Azure AD instance.
1. [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Setup

### 1. Register the web API application in your Azure Active Directory

First, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the sample app.

Use the following settings for your app registration:

| App registration <br/> setting | Value for this sample app                          | Notes                                                                                                       |
|:------------------------------:|:---------------------------------------------------|:------------------------------------------------------------------------------------------------------------|
| **Name**                       | `active-directory-dotnet-webapp-aspnetcore`        | Suggested value for this sample. <br/> You can change the app name at any time.                             |
| **Supported account types**    | **My organization only**                           | Required for this sample. <br/> Support for the Single tenant.                                              |
| **Platform type**              | `Web`                                              | Required value for this sample. <br/> Enables the required and optional settings for the app type.          |
| **Redirect URIs**              | `https://localhost:5001/signin-oidc`               | Required value for this sample. <br/> You can change that later in your own implementation.                 |
| **Front-channel logout URL**   | `https://localhost:5001/signout-oidc`              | Required value for this sample. <br/> You can change that later in your own implementation.                 |
| **Client secret**              | _Value shown in Azure portal_                      | :warning: Record this value immediately! <br/> It's shown only _once_ (when you create it).                 |

> :information_source: **Bold text** in the table matches (or is similar to) a UI element in the Azure portal, while `code formatting` indicates a value you enter into a text box or select in the Azure portal.

<details>
   <summary>:computer: Alternative: Register the application using az-cli</summary>

1. Choose a password for the Azure AD App

    ```bash
    AZURE_AD_APP_SECRET=<at-least-sixteen-characters-here>
    ```

1. Register a new Azure AD App with a reply url

   ```bash
   AZURE_AD_APP_CLIENT_ID_WEBAPP=$(az ad app create --display-name "active-directory-dotnet-webapp-aspnetcore" --password ${AZURE_AD_APP_SECRET} --reply-urls "https://localhost:5001/signin-oidc" --query appId -o tsv) && \
   AZURE_AD_APP_DOMAIN=$(az ad app show --id $AZURE_AD_APP_CLIENT_ID_WEBAPP --query publisherDomain -o tsv)
   ```

1. Setup a Front-channel logout URL

   ```bash
   az ad app update --id $AZURE_AD_APP_CLIENT_ID_WEBAPP --set logoutUrl="https://localhost:5001/signout-oidc"
   ```

</details>

### 2. Configure the web app

1. Set the client secret app

   ```bash
   export AzureAd__ClientSecret=$AZURE_AD_APP_SECRET
   ```

## Run the application

### 1. Run the webapp

1. Execute the following command to get the app up and running:

   ```bash
   dotnet run
   ```

### 2. Signin into the web app

1. Once the web app is listening, navigate to https://localhost:5001
1. Sign-in with your user credentials.

### 3. Clean up

1. Delete the Azure AD app

   ```bash
   az ad app delete --id $AZURE_AD_APP_CLIENT_ID_WEBAPP
   ```

## About the code

The ASP.NET Core 6.0 Web App will allow users to sign-in, so it can retrieve a Security Token scoped specifically for the Microsoft Graph API, and will use that token to access the user's information. For more information about the proposed scenario, please take a look at the following diagram:

```output
                                                  ┌────────────────────────────┐                                         ┌────────────────────────────┐
        https://<fqdn>                            │                            │                                         │                            │
  1──────────────────────────────────────────────►│                            │                                         │                            │
  │            browser redirection                │  ASP.NET Core 6            │   https://graph.microsoft.com/beta/me   │   Microsoft 365            │
  │   2───────────────────────────────────────────┤                            ├─6─────────────────────────────────────► │                            │
  │   │             https://<fqdn>/signin-oidc    │  Web App                   │   Authorization Bearer 1NS...           │   Graph Api                │
  │   │   5──────────────────────────────────────►│                            │                                         │                            │
  │   │   │                                       │                            │                                         │                            │
  │   ▼   │                                       └────────────────────────────┘                                         └────────────────────────────┘
┌─┴───────┴─────────────────┐
│                           │
│                           │
│                           │
│   client (browser)        │
│                           │
│                           │
│                           │
└─────┬─────────────────────┘
      │   ▲
      │   │                                       ┌────────────────────────────┐
      │   │                                       │                            │
      │   │                                       │                            │
      │   │                                       │   Azure AD                 │
      │   │         Security Token (claims)       │                            │
      │   4───────────────────────────────────────┤   STS                      │
      │                                           │                            │
      3──────────────────────────────────────────►│                            │
            https://login.microsoftonline.com/    └────────────────────────────┘

Scenario:

A protected web app allows users to sign in, it enables the possiblity of acquiring and validating their tokens.

Later the web app can make calls to the Microsoft 365 Graph API on behalf of signed-in user.
```

:link: For more information about how to proctect your projects, please let's take a look at https://docs.microsoft.com/en-us/azure/active-directory/develop/sample-v2-code. To know more about how this sample has been generated, please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/?view=aspnetcore-6.0

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
