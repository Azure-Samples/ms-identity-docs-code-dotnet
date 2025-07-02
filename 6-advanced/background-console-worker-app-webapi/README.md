---
page_type: sample
name: Share the MSAL token cache between a web API and a background console worker app
services: ms-identity
platform: DotNet
languages:
  - csharp
  - javascript
products:
 - aspnet-core
 - entra-id
  - microsoft-identity-web
  - microsoft-graph-api
urlFragment: ms-identity-dotnet-advanced-token-cache
description: An ASP.Net Core sample that shows how web APIs can share the token cache with their background services and continue to act on-behalf of users in their absence.
---

# Share the MSAL token cache between a web API and a background console worker app

* [Overview](#overview)
* [Scenario](#scenario)
* [Contents](#contents)
* [Prerequisites](#prerequisites)
* [Setup the sample](#setup-the-sample)
* [Explore the sample](#explore-the-sample)
* [Troubleshooting](#troubleshooting)
* [About the code](#about-the-code)
* [How to deploy this sample to Azure](#how-to-deploy-this-sample-to-azure)
* [Next Steps](#next-steps)
* [Contributing](#contributing)
* [Learn More](#learn-more)

## Overview

This sample shows how a Web API thats is called from a web app thats signs-in users and shares its token cache of signed-in users with a background console application, which can then continue to act on-behalf of users in their absence.

## Scenario

In this sample a .NET Core Web Api protected by [Microsoft Entra](https://azure.microsoft.com/services/active-directory/), calls the [Microsoft Graph API](https://developer.microsoft.com/graph) on behalf-of users using the [on-behalf-of flow](https://learn.microsoft.com/entra/identity-platform/v2-oauth2-on-behalf-of-flow).

It uses the [MSAL.NET](http://aka.ms/msal-net) and [Microsoft.Identity.Web](https://aka.ms/microsoft-identity-web) libraries to sign-in users, acquire an [access token](https://aka.ms/access-tokens) for [Microsoft Graph](https://graph.microsoft.com) and calls the [Microsoft Graph](https://learn.microsoft.com/graph/overview) `/me` endpoint. The [Microsoft.Identity.Web](https://aka.ms/microsoft-identity-web) library is configured to cache the users tokens in a [Sql Server](https://github.com/AzureAD/microsoft-identity-web/wiki/token-cache-serialization) instance.

Another .NET Core console application, which in real world would constitute a background worker for the front-end web app or a Web API, shares the app ID (ClientId) of the Web App and the Web API and thus is able to acquire the access tokens cached earlier by the Web App for [Microsoft Graph](https://learn.microsoft.com/graph/overview) using the MSAL library.

This token sharing allows this console application, which doesn't interact or sign-in users itself, to be able to accomplish its objective of working on behalf of the users who signed-in earlier in the Web App.

Additionally, since the console app uses cached tokens with delegated permissions only, it doesn't need to use a flow like [Client Credentials](https://learn.microsoft.com/entra/identity-platform/v2-oauth2-client-creds-grant-flow) which will necessitate it requesting high-privilege [Application Permissions](https://learn.microsoft.com/graph/auth/auth-concepts#microsoft-graph-permissions) which often require an [admin consent](https://learn.microsoft.com/entra/identity-platform/v2-admin-consent).

![Diagram](ReadmeFiles/diagram.png)

## How to run this sample

### Prerequisites

* Either [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download) and [.NET Core SDK](https://www.microsoft.com/net/learn/get-started)
* An **Microsoft Entra** tenant. For more information, see: [How to get a Microsoft Entra tenant](https://learn.microsoft.com/entra/identity-platform/test-setup-environment#get-a-test-tenant)
* A user account in your **Microsoft Entra** tenant. This sample will not work with a **personal Microsoft account**. If you're signed in to the [Azure portal](https://portal.azure.com) with a personal Microsoft account and have not created a user account in your directory before, you will need to create one before proceeding.

- If you want to store the token cache on a **SQL Server database**, you can easily generate the token cache table by installing the following tool using the **Developer Command Prompt for Visual Studio** (running as an administrator):

    ```shell
    dotnet tool install --global dotnet-sql-cache
    ```

## Setup the sample

### Step 1: Clone or download this repository

From your shell or command line, navigate to the following folder in the repository you cloned earlier

```console
cd "1-Integrated-Cache\1-2-WebAPI-BgWorker"
```

### Install project dependencies

```console
    dotnet restore
```

### Step 3: Register the sample application(s) in your tenant

There are two projects in this sample. Each needs to be separately registered in your Microsoft Entra tenant. To register these projects, you can:

- follow the steps below for manually register your apps
- or use PowerShell scripts that:
  - **automatically** creates the Microsoft Entra applications and related objects (passwords, permissions, dependencies) for you.
  - modify the projects' configuration files.

  <details>
  <summary>Expand this section if you want to use this automation:</summary>

    > :warning: If you have never used **Microsoft Graph PowerShell** before, we recommend you go through the [App Creation Scripts Guide](./AppCreationScripts/AppCreationScripts.md) once to ensure that your environment is prepared correctly for this step.
  
    1. On Windows, run PowerShell as **Administrator** and navigate to the root of the cloned directory
    1. In PowerShell run:

       ```PowerShell
       Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
       ```

    1. Run the script to create your Microsoft Entra application and configure the code of the sample application accordingly.
    1. For interactive process -in PowerShell, run:

       ```PowerShell
       cd .\AppCreationScripts\
   .\Configure.ps1 -TenantId "[Optional] - your tenant id" -AzureEnvironmentName "[Optional] - Azure environment, defaults to 'Global'"
   ```

    > Other ways of running the scripts are described in [App Creation Scripts guide](./AppCreationScripts/AppCreationScripts.md). The scripts also provide a guide to automated application registration, configuration and removal which can help in your CI/CD scenarios.

  </details>

#### Choose the Microsoft Entra tenant where you want to create your applications

To manually register the apps, as a first step you'll need to:

1. Sign in to the [Azure portal](https://portal.azure.com).
1. If your account is present in more than one Microsoft Entra tenant, select your profile at the top right corner in the menu on top of the page, and then **switch directory** to change your portal session to the desired Microsoft Entra tenant.

#### Register the service app (WebApi-SharedTokenCache)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Microsoft Entra ID** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
    1. In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `WebApi-SharedTokenCache`.
    1. Under **Supported account types**, select **Accounts in this organizational directory only**
    1. Select **Register** to create the application.
1. In the **Overview** blade, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
1. In the app's registration screen, select the **Certificates & secrets** blade in the left to open the page where you can generate secrets and upload certificates.
1. In the **Client secrets** section, select **New client secret**:
    1. Type a key description (for instance `app secret`).
    1. Select one of the available key durations (**6 months**, **12 months** or **Custom**) as per your security posture.
    1. The generated key value will be displayed when you select the **Add** button. Copy and save the generated value for use in later steps.
    1. You'll need this key later in your code's configuration files. This key value will not be displayed again, and is not retrievable by any other means, so make sure to note it from the Azure portal before navigating to any other screen or blade.
   1. In the app's registration screen, select the **API permissions** blade in the left to open the page where we add access to the APIs that your application needs:
   1. Select the **Add a permission** button and then,
   1. Ensure that the **Microsoft APIs** tab is selected.
   1. In the *Commonly used Microsoft APIs* section, select **Microsoft Graph**
      * Since this app signs-in users, we will now proceed to select **delegated permissions**, which is is requested by apps when signing-in users.
           1. In the **Delegated permissions** section, select the **User.Read** in the list. Use the search box if necessary.
   1. Select the **Add permissions** button at the bottom.
1. In the app's registration screen, select the **Expose an API** blade to the left to open the page where you can publish the permission as an API for which client applications can obtain [access tokens](https://aka.ms/access-tokens) for. The first thing that we need to do is to declare the unique [resource](https://learn.microsoft.com/entra/identity-platform/v2-oauth2-auth-code-flow) URI that the clients will be using to obtain access tokens for this API. To declare an resource URI(Application ID URI), follow the following steps:
    1. Select **Set** next to the **Application ID URI** to generate a URI that is unique for this app.
    1. For this sample, accept the proposed Application ID URI (`api://{clientId}`) by selecting **Save**. Read more about Application ID URI at [Validation differences by supported account types \(signInAudience\)](https://learn.microsoft.com/entra/identity-platform/supported-accounts-validation).
 
##### Publish Delegated Permissions

1. All APIs must publish a minimum of one [scope](https://learn.microsoft.com/entra/identity-platform/v2-oauth2-auth-code-flow#request-an-authorization-code), also called [Delegated Permission](https://learn.microsoft.com/entra/identity-platform/v2-permissions-and-consent#permission-types), for the client apps to obtain an access token for a *user* successfully. To publish a scope, follow these steps:
1. Select **Add a scope** button open the **Add a scope** screen and Enter the values as indicated below:
    1. For **Scope name**, use `Profile.Read`.
    1. Select **Admins and users** options for **Who can consent?**.
    1. For **Admin consent display name** type in the details, `e.g. Allow the users of the app WebApi-SharedTokenCache to read ToDo list items`.
    1. For **Admin consent description** type in the details `e.g. Allows the app WebApi-SharedTokenCache to read the signed-in users ToDo list items.`
    1. For **User consent display name** type in the details `e.g. Read ToDo list items as yourself`.
    1. For **User consent description** type in the details `e.g. Allow the app WebApi-SharedTokenCache to read ToDo list items on your behalf.`
    1. Keep **State** as **Enabled**.
    1. Select the **Add scope** button on the bottom to save this scope.
    > Repeat the steps above for another scope named **Profile.ReadWrite**
1. Select the **Manifest** blade on the left.
    1. Set `accessTokenAcceptedVersion` property to **2**.
    1. Select on **Save**.

    > :information_source:  Follow  [the principle of least privilege](https://learn.microsoft.com/entra/identity-platform/secure-least-privileged-access) whenever you are publishing permissions for a web API.

##### Configure Optional Claims

1. Still on the same app registration, select the **Token configuration** blade to the left.
1. Select **Add optional claim**:
    1. Select **optional claim type**, then choose **Access**.
    1. Select the optional claim **idtyp**.
    1. Select **Add** to save your changes.

##### Configure the service app (WebApi-SharedTokenCache) to use your app registration

Open the project in your IDE (like Visual Studio or Visual Studio Code) to configure the code.

> In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `WebAPI\appsettings.json` file.
1. Find the key `Domain` and replace the existing value with your Microsoft Entra tenant name.
1. Find the key `TenantId` and replace the existing value with your Microsoft Entra tenant ID.
1. Find the key `ClientId` and replace the existing value with the application ID (clientId) of `WebApi-SharedTokenCache` app copied from the Azure portal.
1. Find the key `ClientSecret` and replace the existing value with the key you saved during the creation of `WebApi-SharedTokenCache` copied from the Azure portal.

#### Register the client app (ProfileSPA-SharedTokenCache)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Microsoft Entra ID** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
    1. In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `ProfileSPA-SharedTokenCache`.
    1. Under **Supported account types**, select **Accounts in this organizational directory only**
    1. Select **Register** to create the application.
1. In the **Overview** blade, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
1. In the app's registration screen, select the **Authentication** blade to the left.
1. If you don't have a platform added, select **Add a platform** and select the **Single-page application** option.
    1. In the **Redirect URI** section enter the following redirect URIs:
        1. `http://localhost:3000`
        1. `http://localhost:3000/redirect.html`
    1. Click **Save** to save your changes.
1. Since this app signs-in users, we will now proceed to select **delegated permissions**, which is is required by apps signing-in users.
   1. In the app's registration screen, select the **API permissions** blade in the left to open the page where we add access to the APIs that your application needs:
   1. Select the **Add a permission** button and then:

   1. Ensure that the **Microsoft APIs** tab is selected.
   1. In the *Commonly used Microsoft APIs* section, select **Microsoft Graph**
      * Since this app signs-in users, we will now proceed to select **delegated permissions**, which is is requested by apps when signing-in users.
           1. In the **Delegated permissions** section, select the **User.Read** in the list. Use the search box if necessary.
   1. Select the **Add permissions** button at the bottom.
   1. Select the **Add a permission** button and then:

   1. Ensure that the **My APIs** tab is selected.
   1. In the list of APIs, select the API `WebApi-SharedTokenCache`.
      * Since this app signs-in users, we will now proceed to select **delegated permissions**, which is is requested by apps when signing-in users.
           1. In the **Delegated permissions** section, select the **Profile.Read**, **Profile.ReadWrite** in the list. Use the search box if necessary.
   1. Select the **Add permissions** button at the bottom.

##### Configure the client app (ProfileSPA-SharedTokenCache) to use your app registration

Open the project in your IDE (like Visual Studio or Visual Studio Code) to configure the code.

> In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `SPA\src\authConfig.js` file
1. Find the app key `clientId` and replace the existing value with the application ID (clientId) of the `ProfileSPA-SharedTokenCache` application copied from the Azure portal.
1. Find the app key `authority` and replace the existing value with ('https://login.microsoftonline.com/'+ $tenantId).
1. Find the app key `protectedResources.endpoint` and replace the existing value with the base address of the WebApi-SharedTokenCache project (by default `https://localhost:44351/api/profile`).
2. Find the app key `protectedResources.scopes` and replace the existing value with the `/.default` scope: `api://{client_id}/.default`. [Learn more about the /.default scope](https://learn.microsoft.com/entra/identity-platform/v2-permissions-and-consent#the-default-scope)

#### Configure Known Client Applications for service (WebApi-SharedTokenCache)

For a middle-tier web API (`WebApi-SharedTokenCache`) to be able to call a downstream web API, the middle tier app needs to be granted the required permissions as well. However, since the middle-tier cannot interact with the signed-in user, it needs to be explicitly bound to the client app in its **Microsoft Entra** registration. This binding merges the permissions required by both the client and the middle-tier web API and presents it to the end user in a single consent dialog. The user then consent to this combined set of permissions. To achieve this, you need to add the **Application Id** of the client app to the `knownClientApplications` property in the **manifest** of the web API. Here's how:

1. In the [Azure portal](https://portal.azure.com), navigate to your `WebApi-SharedTokenCache` app registration, and select the **Manifest** blade.
1. In the manifest editor, change the `knownClientApplications: []` line so that the array contains the Client ID of the client application (`ProfileSPA-SharedTokenCache`) as an element of the array.

For instance:

```json
    "knownClientApplications": ["ca8dca8d-f828-4f08-82f5-325e1a1c6428"],
```

1. **Save** the changes to the manifest.

## Step 3: Register the Background Worker project with your Microsoft Entra tenant

In order to have the Web App and the Background Worker sharing the same token cache, **they must share the same application ID (clientId)** on Microsoft Entra as well. So for this step, you will set additional configuration to the existing `WebApi-SharedTokenCache` app registration.

1. Navigate to your `WebApi-SharedTokenCache` [App registration](https://go.microsoft.com/fwlink/?linkid=2083908) page.
2. In the app's registration screen, click on the **Authentication** blade in the left.
3. In the **Authentication** section, click on **Add a platform**:
    - Choose **Mobile and desktop applications**
    - Under **Redirect URIs** select the option, `https://login.microsoftonline.com/common/oauth2/nativeclient`
    - Select **Configure** in the bottom

### Step 3.1: Configure the Background Worker project to use your app registration

Open the project in your IDE (like Visual Studio) to configure the code.
>In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `appsettings.json` file for the `BackgroundWorker`.
2. Find the app key `ClientId` and replace the existing value with the application ID (clientId) of the `WebApi-SharedTokenCache` copied from the Azure portal.
3. Find the app key `TenantId` and replace the value with the Tenant ID where you registered your application.
4. Find the app key `Domain` and replace the existing value with your Microsoft Entra tenant name.
5. Find the app key `ClientSecret` and replace the existing value with the key you saved during the creation of the `WebApi-SharedTokenCache`, in the Azure portal.
6. Find the section for the `ConnectionStrings` and replace the value of the keys that are relevant to your scenario:
   - If you will use **SQL Server**, update the key `TokenCacheDbConnStr` with the database connection string.
   - If you will use **Redis**, update the key `TokenCacheRedisConnStr` with the Redis connection string, and the key `TokenCacheRedisInstaceName` with the the Redis instance name.

### Storing the token cache on Redis

>NOTE: If you are storing the token cache on SQL Server, you can skip this step.

If you are storing the distributed token cache on Redis, you will need to modify the *BackgroundWorker* `Program.cs` file, and *WebAPI* `Startup.cs` file:

- Open the file `Program.cs`
  - Comment the section named **SQL SERVER CONFIG**
  - Uncomment the section named **REDIS CONFIG**
- Open the file `Startup.cs`
  - Comment the section named **SQL SERVER CONFIG**
  - Uncomment the section named **REDIS CONFIG**

## Step 4: Run the sample

To populate the distributed token cache, and the entity `MsalAccountActivity`, the **SPA and WebApi must be executed first**. 

### Step 4.1: Run WebApi project

Run the **WebApi** project.

When you start the Web API from Visual Studio, depending on the browser you use, you'll get:

- an empty web page (case with Microsoft Edge)
- or an error HTTP 401 (case with Chrome)

This behavior is expected as you are not authenticated and the WebApi doesn't have a landing page.

### Step 4.2: Run and Install SPA dependencies

Using a command line interface such as VS Code integrated terminal, locate the application directory and execute the following command:

```console
cd ../
cd SPA
npm install
```

Once `npm install` is completed, you can run the SPA with the command:

```console
npm start
```

Open the SPA in either different browsers or use the same browser but in incognito mode, sign-in with multiple users and click on the **Call Web API** button.

Once you have signed-in with at least 2 users you can stop both SPA and WebAPI projects, and execute the BackgroundWorker project.

Once running, the background worker should be returning all account activities that happened more than 30 seconds ago. You can either change the time interval or wait for it.

With all the accounts retrieved, the background worker will print those that got their token acquired successfully and those that failed. To test a failure scenario, you can sign-out one of the users in the WebApp, and execute the background worker again.

With all the accounts retrieved, the background worker will print those that got their token acquired successfully and those that failed. To test a failure scenario, you can for instance, delete one of the users token cache DB and execute the background worker again, or wait for its sliding expiration time and try again (the sliding expiration is set on **Startup.cs**).

> [Consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUM0FERTFGTjlKVUVWTzNINjdMTkFWTE43UiQlQCN0PWcu)

## About The code

### IntegratedCacheUtils project

[Please refer to this page to learn more about the IntegratedCacheUtils project.](../IntegratedCacheUtilsReadme.md)

### SPA and Web Api project

In the Web Api project, we leverage [`Microsoft.Identity.Web`](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki) to configure the bearer authentication and on-behalf-of flow(OBO), and use 
the `IntegratedTokenCacheAdapter.cs` as an extension for the `MsalDistributedTokenCacheAdapter`, so that before MSAL writes a token cache, we hydrate and save the `MsalAccountActivity.cs` entity. 

```c#
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddIntegratedUserTokenCache();

            // Add a custom GraphServiceClient which leverages OBO keys for persistence.
            services.AddCustomMicrosoftGraphClient(Configuration);

To store the distributed token cache, you could use SQL Server or Redis for instance:

SQL Server, leveraging `Microsoft.Extensions.Caching.Sql` NuGet package:

```c#
services.AddDistributedSqlServerCache(options =>
{
    /*
        To create the token cache table:
        dotnet tool install --global dotnet-sql-cache
        dotnet sql-cache create "<YourDatabaseConnectionString>" dbo <TokenCacheTableName>    
    */
    options.ConnectionString = Configuration.GetConnectionString("TokenCacheDbConnStr");
    options.SchemaName = "dbo";
    options.TableName = "<TokenCacheTableName>";
    //Once expired, the cache entry is automatically deleted by Microsoft.Extensions.Caching.SqlServer library
    options.DefaultSlidingExpiration = TimeSpan.FromHours(2);
});
```

Redis, leveraging `Microsoft.Extensions.Caching.StackExchangeRedis` NuGet package:

```c#
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Configuration.GetConnectionString("TokenCacheRedisConnStr");
    options.InstanceName = Configuration.GetConnectionString("TokenCacheRedisInstaceName");
});
```

And setup the Dependency Injection for the desired `IMsalAccountActivityStore` implementation. For instance:

Save `MsalAccountActivity.cs` entity on SQL Server:

```c#
services.AddDbContext<IntegratedTokenCacheDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("TokenCacheDbConnStr")));
services.AddScoped<IIntegratedTokenCacheStore, IntegratedSqlServerTokenCacheStore>();
```

### BackgroundWorker project

[Please refer to this page to learn more about the BackgroundWorker project.](../BackgroundWorker.md)

## Learn more

- Learn about [Microsoft.Identity.Web](https://github.com/AzureAD/microsoft-identity-web/wiki)
- Learn how to enable distributed caches in [token cache serialization](https://github.com/AzureAD/microsoft-identity-web/wiki/token-cache-serialization)
- [Use HttpClientFactory to implement resilient HTTP requests](https://learn.microsoft.com/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests) used by the Graph custom service