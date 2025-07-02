---
services: active-directory
platforms: dotnet
author: Shama-K
level: 100
client: .NET Desktop (Console)
service: Microsoft Graph
endpoint: Microsoft identity platform
page_type: sample
languages:
  - csharp  
products:
  - azure
  - microsoft-entra-id  
  - dotnet
  - office-ms-graph
description: "This sample demonstrates a .NET Desktop (Console) application authenticating a user with the device code flow"
---
# Sign-in a user with the Microsoft identity platform using the device code flow and call Microsoft Graph.

![.NET Core](https://github.com/Azure-Samples/ms-identity-dotnet-desktop-tutorial/workflows/.NET%20Core/badge.svg)

## About this sample

### Overview

This sample demonstrates a .NET Desktop (Console) application  authenticating a user with the [device code flow](https://learn.microsoft.com/entra/identity-platform/v2-oauth2-device-code) and calling Microsoft Graph on behalf of the user. This flow allows users to sign in to input-constrained devices such as a smart TV, IoT device, or printer. To enable this flow, the device has the user visit a webpage in their browser on another device to sign in. Once the user signs in, the device is able to get access tokens and refresh tokens as needed.

1. The .NET Desktop (Console) application uses the [Microsoft Authentication Library (MSAL)](https://learn.microsoft.com/entra/identity-platform/msal-overview) to sign-in a user and obtains an [access token](https://learn.microsoft.com/entra/identity-platform/access-tokens) for Microsoft Graph from Microsoft Entra ID. The user is  using the [device code flow](https://learn.microsoft.com/entra/identity-platform/v2-oauth2-device-code).
2. The access token is used as a bearer token to authenticate the user when calling Microsoft Graph.

> Looking for previous versions of this code sample? Check out the tags on the [releases](../../releases) GitHub page.

![Overview](./ReadmeFiles/topology.png)

### Scenario

This console application displays a code and a URL which the user will open in a browser on a device with an available input device (keyboard). The user will open the URL in the browser and enter the code to start the authentication process.
Once the authentication process completes, the console application will resume and call Microsoft Graph on behalf of the user to fetch some information and prints it on the screen.

## How to run this sample

To run this sample, you'll need:

- [Visual Studio 2019](https://aka.ms/vsdownload)
- An Internet connection
- a Microsoft Entra tenant. For more information on how to get a Microsoft Entra tenant, see [How to get a Microsoft Entra tenant](https://azure.microsoft.com/documentation/articles/active-directory-howto-tenant/)
- A user account in your Microsoft Entra tenant. This sample will not work with a Microsoft account (formerly Windows Live account). Therefore, if you signed in to the [Microsoft Entra admin center](https://entra.microsoft.com) with a Microsoft account and have never created a user account in your directory before, you need to do that now.

### Step 1:  Clone or download this repository

From your shell or command line:

```Shell
git clone https://github.com/Azure-Samples/ms-identity-dotnet-desktop-tutorial.git
cd "4-DeviceCodeFlow"
```

or download and extract the repository .zip file.

> Given that the name of the sample is quiet long, and so are the names of the referenced NuGet packages, you might want to clone it in a folder close to the root of your hard drive, to avoid the 256 character path length limitation on Windows.

### Step 2:  Register the sample application with your Microsoft Entra tenant

There is one project in this sample. To register it, you can:

- either follow the steps [Step 2: Register the sample with your Microsoft Entra tenant](#step-2-register-the-sample-with-your-azure-active-directory-tenant) and [Step 3:  Configure the sample to use your Microsoft Entra tenant](#choose-the-azure-ad-tenant-where-you-want-to-create-your-applications)
- or use PowerShell scripts that:
  - **automatically** creates the Microsoft Entra applications and related objects (passwords, permissions, dependencies) for you. Note that this works for Visual Studio only.
  - modify the Visual Studio projects' configuration files.

<details>
  <summary>Expand this section if you want to use this automation:</summary>

1. On Windows, run PowerShell and navigate to the root of the cloned directory
1. In PowerShell run:

   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```

1. Run the script to create your Microsoft Entra application and configure the code of the sample application accordingly.
1. In PowerShell run:

   ```PowerShell
   cd .\AppCreationScripts\
   .\Configure.ps1
   ```

   > Other ways of running the scripts are described in [App Creation Scripts](./AppCreationScripts/AppCreationScripts.md)
   > The scripts also provide a guide to automated application registration, configuration and removal which can help in your CI/CD scenarios.

1. Open the Visual Studio solution and click start to run the code.

</details>

Follow the steps below to manually walk through the steps to register and configure the applications in the Microsoft Entra admin center.

#### Choose the Microsoft Entra tenant where you want to create your applications

As a first step you'll need to:

1. Sign in to the [Microsoft Entra admin center](https://entra.microsoft.com) using either a work or school account or a personal Microsoft account.
1. If your account is present in more than one Microsoft Entra tenant, select your profile at the top right corner in the menu on top of the page. Then select **switch directory** to change your portal session to the desired Microsoft Entra tenant.

#### Register the client app (Console-DeviceCodeFlow-MultiTarget-v2)

1. Navigate to the Microsoft identity platform for developers [App registrations](https://go.microsoft.com/fwlink/?linkid=2083908) page.
1. Select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `Console-DeviceCodeFlow-MultiTarget-v2`.
   - Under **Supported account types**, select **Accounts in this organizational directory only**.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
   - In the **Advanced settings** | **Default client type** section, flip the switch for `Treat application as a public client` to **Yes**.
1. Select **Save** to save your changes.
1. In the app's registration screen, click on the **API permissions** blade in the left to open the page where we add access to the Apis that your application needs.
   - Click the **Add a permission** button and then,
   - Ensure that the **Microsoft APIs** tab is selected.
   - In the *Commonly used Microsoft APIs* section, click on **Microsoft Graph**
   - In the **Delegated permissions** section, select the **User.Read** in the list. Use the search box if necessary.
   - Click on the **Add permissions** button at the bottom.

##### Configure the  client app (Console-DeviceCodeFlow-MultiTarget-v2) to use your app registration

Open the project in your IDE (like Visual Studio) to configure the code.
>In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `Console-DeviceCodeFlow-v2\appsettings.json` file
1. Find the app key `ClientId` and replace the existing value with the application ID (clientId) of the `Console-DeviceCodeFlow-MultiTarget-v2` application copied from the Microsoft Entra admin center.
1. Find the app key `TenantId` and replace the existing value with your Microsoft Entra tenant ID.

### Step 4: Run the sample

Clean the solution, rebuild the solution, and run it.

Use a web browser to open the Url (https://microsoft.com/devicelogin) that is displayed in console app. Input the code presented in the console , sign-in and check the result of the operation back in the console.

> [Consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUREhEVDBOTFBMUVRPUElBUE5WMjdPQ1RaMiQlQCN0PWcu)

## About the code

The relevant code for this sample is in the `Program.cs` file, in the Main() method. The steps are:

1- We use the  `appsettings.json` as our configuration file and build the **PublicClientApplicationOptions** object with the app registration settings

```csharp
var builder = new ConfigurationBuilder()
    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

configuration = builder.Build();

appConfiguration = configuration.Get<PublicClientApplicationOptions>();
```

2- The method **SignInUserAndGetTokenUsingMSAL** contains the code to initialize MSAL and get an access token for MS Graph.
Try to acquire an access token for Microsoft Graph silently, but if it fails, do it using `AcquireTokenWithDeviceCode()`.
This method will give you code, which will have the lifetime of 15 minutes, and URL for authentication.

```csharp
private static async Task<string> SignInUserAndGetTokenUsingMSAL(PublicClientApplicationOptions configuration, string[] scopes)
{
    // build the Microsoft Entra authority Url
    string authority = string.Concat(configuration.Instance, configuration.TenantId);

    // Initialize the MSAL library by building a public client application
    application = PublicClientApplicationBuilder.Create(configuration.ClientId)
                                            .WithAuthority(authority)
                                            .WithDefaultRedirectUri()
                                            .Build();

    AuthenticationResult result;

    try
    {
        var accounts = await application.GetAccountsAsync();
        result = await application.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
    }
    catch (MsalUiRequiredException)
    {
        result = await application.AcquireTokenWithDeviceCode(scopes, deviceCodeResult =>
           {
               Console.WriteLine(deviceCodeResult.Message);
               return Task.FromResult(0);
           }).ExecuteAsync();
    }
    return result.AccessToken;
}
```

3- The method **SignInAndInitializeGraphServiceClient** initializes the Graph SDK with the access token we obtained earlier in **SignInUserAndGetTokenUsingMSAL**.

```csharp
private async static Task<GraphServiceClient> SignInAndInitializeGraphServiceClient(PublicClientApplicationOptions configuration, string[] scopes)
{
    GraphServiceClient graphClient = new GraphServiceClient(MSGraphURL,
        new DelegateAuthenticationProvider(async (requestMessage) =>
        {
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", await SignInUserAndGetTokenUsingMSAL(configuration, scopes));
        }));

    return await Task.FromResult(graphClient);
}
```

4- The method **CallMSGraph** uses the initialized Graph SDK to make a call to Graph and fetch data from it.
```csharp
private static async Task CallMSGraph(GraphServiceClient graphClient)
{
    var me = await graphClient.Me.Request().GetAsync();

    // Printing the results
    Console.Write(Environment.NewLine);
    Console.WriteLine("-------- Data from call to MS Graph --------");
    Console.Write(Environment.NewLine);
    Console.WriteLine($"Id: {me.Id}");
    Console.WriteLine($"Display Name: {me.DisplayName}");
    Console.WriteLine($"Email: {me.Mail}");
}
```

## Community Help and Support

Use [Stack Overflow](http://stackoverflow.com/questions/tagged/msal) to get support from the community.
Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before.
Make sure that your questions or comments are tagged with [`azure-active-directory` `msal` `dotnet`].

If you find a bug in the sample, please raise the issue on [GitHub Issues](../../issues).

To provide a recommendation, visit the following [User Voice page](https://feedback.azure.com/forums/169401-azure-active-directory).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## More information

For more information, see MSAL.NET's conceptual documentation:

- [MSAL.NET's conceptual documentation](https://aka.ms/msal-net)
- [Device code flow](https://learn.microsoft.com/entra/identity-platform/v2-oauth2-device-code)
- [Microsoft identity platform (Microsoft Entra ID for developers)](https://learn.microsoft.com/entra/identity-platform/)
- [Quickstart: Register an application with the Microsoft identity platform](https://learn.microsoft.com/entra/identity-platform/quickstart-register-app)
- [Quickstart: Configure a client application to access web APIs](https://learn.microsoft.com/entra/identity-platform/quickstart-configure-app-access-web-apis)
- [Understanding Microsoft Entra application consent experiences](https://learn.microsoft.com/entra/identity-platform/application-consent-experience)
- [Understand user and admin consent](https://learn.microsoft.com/entra/identity-platform/howto-convert-app-to-be-multi-tenant#understand-user-and-admin-consent)
- [Application and service principal objects in Microsoft Entra ID](https://learn.microsoft.com/entra/identity-platform/app-objects-and-service-principals)
- [Acquiring Tokens](https://aka.ms/msal-net-acquiring-tokens)
- [National Clouds](https://learn.microsoft.com/entra/identity-platform/authentication-national-cloud#app-registration-endpoints)

For more information about how OAuth 2.0 protocols work in this scenario and other scenarios, see [Authentication Scenarios for Microsoft Entra ID](http://go.microsoft.com/fwlink/?LinkId=394414).
