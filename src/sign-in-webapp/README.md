# ASP.NET Core 6.0 Web App - Sign-in user | Microsoft identity platform

The web app in this scenario has been created using the ASP.NET Core 6.0 Razor template, and slightly modified to add authentication enabling the users sign-in that follows the [Open Id Connect](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc) standard protocol. To lite up Open Id, it is using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) middlewares.  In other words, a simple web app is secured by adding an authentication layer allowing users to sign-in with their Work and school (Azure AD) accounts, and as a result it can make web API calls to protected resources on behalf of the signed-in user. For more information about the proposed scenario, please take a look at the following diagram:

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

## Prerequisites

1. An Azure Active Directory (Azure AD) tenant. You can [open an Azure account for free](https://azure.microsoft.com/free) to get an Azure AD instance.
1. [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Register the web API application in your Azure Active Directory

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

## Configure the web app

1. Set the client secret app

   ```bash
   export AzureAd__ClientSecret=$AZURE_AD_APP_SECRET
   ```

## Run the web API

1. Execute the following command to get the app up and running:

   ```bash
   dotnet run
   ```

## Signin into the web app

1. Once the web app is listening, navigate to https://localhost:5001

## Clean up

1. Delete the Azure AD app

   ```bash
   az ad app delete --id $AZURE_AD_APP_CLIENT_ID_WEBAPP
   ```
