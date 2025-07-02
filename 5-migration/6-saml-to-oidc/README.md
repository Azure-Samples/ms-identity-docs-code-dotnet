
# Migrate a .NET application using SAML protocol to OpenID Connect

## Scenario

You have a web application using SAML protocol on Microsoft Entra and you would like to migrate it to use OpenID Connect protocol.

Here we migrate the provided ASP.NET web application that uses the [SAML](https://learn.microsoft.com/entra/identity-platform/single-sign-on-saml-protocol) protocol to authenticate users and registered in your Microsoft Entra tenant to the [OAuth 2.0 and OpenID Connect protocol](https://learn.microsoft.com/entra/identity-platform/active-directory-v2-protocols). The web application project can be found in [chapter 1-1](https://github.com/Azure-Samples/ms-identity-dotnet-adfs-to-aad/tree/master/1-ADFS-Host/1-1-Setup-SAML-Playground/README.md).

This sample uses the `Microsoft.Owin.Security.WsFederation` library for authenticating users using SAML, which we will change to use the `Microsoft.Owin.Security.OpenIdConnect` library instead.

Application developers might consider this to enable their applications to be able to work with [OAuth 2.0](https://learn.microsoft.com/entra/identity-platform/v2-app-types) based Web APIs like [Microsoft Graph](https://learn.microsoft.com/graph/overview) and [Azure REST API](https://learn.microsoft.com/rest/api/azure/).

### Prerequisites

- [Visual Studio](https://aka.ms/vsdownload)
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework)
- A Microsoft Entra tenant. For more information on how to get a Microsoft Entra tenant, see [How to get a Microsoft Entra tenant](https://learn.microsoft.com/entra/identity-platform/quickstart-create-new-tenant)

## Migrate from SAML to OpenID Connect

Azure Microsoft Entra supports application using both [SAML protocol](https://learn.microsoft.com/entra/identity-platform/single-sign-on-saml-protocol) and [OAuth 2.0 and OpenID Connect protocol](https://learn.microsoft.com/entra/identity-platform/active-directory-v2-protocols) for authentication.

If you have a application using SAML registered on Microsoft Entra today, you can use the same application registration to enable authentication using [OAuth 2.0 and OpenID Connect protocol](https://learn.microsoft.com/entra/identity-platform/active-directory-v2-protocols), thus requiring minimal code changes.

### Application changes in Microsoft Entra ID

First, sign in to the [Azure portal](https://portal.azure.com) and:

1. Record the `TenantId`, which can be found in the [Microsoft Entra menu](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/Overview)
2. Record the `ClientId` of the SAML application that is already registered on Microsoft Entra. To find it, navigate to **App Registrations**, search for the application name and copy the value for the **Application (client) ID** column.
3. Add Microsoft Graph delegated permission `User.Read` to the application:
    - Open the application registration
    - Navigate to **API Permissions** on the left blade
    - Select **Add a permission**
    - Click on **Microsoft Graph** option
    - Select **Delegated permissions**
    - Search for `User.Read` and select it
    - Select **Add permissions** button

### (Optional) Configure security groups as claims

1. Search and select the application where you want to include Security Groups. For instance, `WebApp_SAML`.
1. Navigate to **Token configuration** on the left blade.
1. If there isn't a configuration for groups yet, select **Add groups claim**, otherwise edit the existing configuration.
1. The following blade is divided by the sections **ID**, **Access** and **SAML**. Each section correspond to the groups claim configuration for the **Id Token**, **Access Token** and **SAML** claims issued by the application respectively. For each section that you would like to include group claims, select the group property that the claim should return. [Learn more details about group claim](https://learn.microsoft.com/entra/identity-platform/active-directory-optional-claims#configuring-groups-optional-claims).
1. Select **Save**.

### Code Changes

Open the web application, and in the *web.config* file, add the following keys changing the values with the ones obtained previously:

```xml
    <add key="ida:ClientId" value="{Client_Id}" />
    <add key="ida:TenantId" value="{TenantId}" />
    <add key="ida:AADInstance" value="https://login.microsoftonline.com/" />
    <add key="ida:RedirectUri" value="https://localhost:44347/" />
```

1. Import the NuGet package `Microsoft.Owin.Security.OpenIdConnect`.
1. Open the *Startup.Auth.cs* file under the *App_Start* folder.
1. Add the following variables

    ```c#
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string authority = String.Concat(aadInstance, tenantId, "/v2.0");
    ```

1. Replace the code for `app.UseWsFederationAuthentication` to

    ```c#
        app.UseOpenIdConnectAuthentication(
        new OpenIdConnectAuthenticationOptions
        {
            Authority = authority,
            ClientId = clientId,
            RedirectUri = redirectUri,
            PostLogoutRedirectUri = redirectUri,
            MetadataAddress = $"https://login.microsoftonline.com/{tenantId}/.well-known/openid-configuration?appid={clientId}"
        });
    ```

1. Open the *AccountController.cs* file and change `WsFederationAuthenticationDefaults.AuthenticationType` to `OpenIdConnectAuthenticationDefaults.AuthenticationType`.

### Test the application

Clean and build the solution, then run the web application and sign-in with a user existing in your tenant. The user will get authenticated and the claims containing in the **Id Token** will be displayed in the screen.

> If you find a bug in the sample, please raise the issue on [GitHub Issues](../../issues).

## We'd love your feedback!

Were we successful in addressing your learning objective? [Do consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUODFCRVg4VTk2QUE2VEFPMUZKSEJNUFhWUyQlQCN0PWcu)

We're always listening, and if you want to get in touch with you directly, send an email to <aadappfeedback@microsoft.com>.

### Useful resources

- [Moving application authentication from AD FS to Microsoft Entra ID](https://learn.microsoft.com/azure/active-directory/manage-apps/migrate-adfs-apps-to-azure)
- [Configure SAML-based single sign-on to non-gallery applications](https://learn.microsoft.com/azure/active-directory/manage-apps/configure-single-sign-on-non-gallery-applications)
