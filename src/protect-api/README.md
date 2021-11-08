# Protected ASP.NET Core minimal API | Microsoft identity platform

The sample code provided here has been created using minimal web API in ASP.NET Core 6.0, and slightly modified to be protected for a single organization using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) that interacts with [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview).  In other words, a very minimalist web api is secured by adding an authorization layer before user requests can reach protected resources.  At this point it is expected that the user authentication had already happened, so a token containing user's information is being sent in the request headers to be used in the authorization process.

:link: For more information about how to proctect your projects, please let's take a look at https://docs.microsoft.com/en-us/azure/active-directory/develop/sample-v2-code. To know more about how this sample has been generated, please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code

## Prerequisites

1. [Download .NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Register the web API application in your Azure Active Directory

1. Register a new Azure AD App

   ```bash
   AZURE_AD_APP_DETAILS_MINIMAL_API=$(az ad app create --display-name "active-directory-dotnet-minimal-api-aspnetcore" -o json) && \
   AZURE_AD_APP_CLIENT_ID_MINIMAL_API=$(echo $AZURE_AD_APP_DETAILS_MINIMAL_API | jq ".appId" -r)
   ```

1. Disable the default scope for `user_impersonation`

   ```bash
   AZURE_AD_APP_USER_IMPERSONATION_SCOPE=$(echo $AZURE_AD_APP_DETAILS_MINIMAL_API | jq '.oauth2Permissions[0].isEnabled = false' | jq -r '.oauth2Permissions') && \
   az ad app update --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API --set oauth2Permissions="$AZURE_AD_APP_USER_IMPERSONATION_SCOPE"
   ```

1. Create a new manifest scope for `access_as_user`

   ```bash
   cat > access_as_user_scope.json <<EOF
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
       "value": "access_as_user"
     }
   ]
   EOF
   ```

1. Set the api uri and the `access_as_user` scope

   ```bash
   az ad app update --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API --identifier-uris "api://${AZURE_AD_APP_CLIENT_ID_MINIMAL_API}" --set oauth2Permissions=@access_as_user_scope.json
   ```

## Scaffold the web API by using the ASP.NET Core Minimal Api project template

1. Execute the following command to create the new web api project

   ```bash
   dotnet new webapi -minimal -o <name>
   ```

1. Add donet package for ASP.NET Core Identity

   ```bash
   dotnet add package Microsoft.Identity.Web
   ```

## Configure the web API

1. Create the `appsettings.json` file with the Azure AD app comfiguration

   ```bash
   cat > appsettings.json <<EOF
   {
     "AzureAd": {
       "Instance": "https://login.microsoftonline.com/",
       "ClientId": "${AZURE_AD_APP_CLIENT_ID_MINIMAL_API}",
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

## Run the web API

1. Execute the following command to get the app up and running:

   ```bash
   dotnet run
   ```

## Send request to the web API

1. Once the app is listening, execute the following to send the a request.

   ```bash
   curl -X GET https://localhost:5001/weatherforecast -ki
   ```

   :book: Since the request is sent without a Bearer Token, it is expected to receive an Unauthorized reponse `401`. The web API is now protected

## Clean up

1. Delete the Azure AD app

   ```bash
   az ad app delete --id $AZURE_AD_APP_CLIENT_ID_MINIMAL_API
   ```
