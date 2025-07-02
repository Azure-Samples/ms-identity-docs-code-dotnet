# Migrating a .NET MVC application using SAML protocol from AD FS to Microsoft Entra ID

## Scenario

Here we migrate the provided ASP.NET web application that uses the [SAML](https://learn.microsoft.com/entra/identity-platform/single-sign-on-saml-protocol) protocol to authenticate users and integrated with AD FS, to your Microsoft Entra tenant.

## About the sample

In the previous [chapter 1-1](https://github.com/Azure-Samples/ms-identity-dotnet-adfs-to-aad/tree/master/1-ADFS-Host/1-1-Setup-SAML-Playground/README.md) we integrated an ASP.NET web application with an AD FS instance.

After the migration, this sample will use the `App Federation Metadata Url` from the Microsoft Entra tenant, for authentication.

### Prerequisites

- [Visual Studio](https://aka.ms/vsdownload)
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework)
- An AD FS environment
- A Microsoft Entra tenant. For more information on how to get a Microsoft Entra tenant, see [How to get a Microsoft Entra tenant](https://learn.microsoft.com/entra/identity-platform/quickstart-create-new-tenant)

## Migrate the SAML application from ADFSto Microsoft Entra ID

### Register the SAML application in Microsoft Entra ID

1. Sign in to the [Azure portal](https://portal.azure.com) using a tenant admin account.
1. Navigate to the Microsoft identity platform for developers [Enterprise applications](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/EnterpriseApps) page.
1. Select **New registration**.
1. Select **Non gallery app**.
1. Choose a name for the application, for instance `WebApp_SAML` and select **Add** on the bottom.
1. Under the Manage section, select **Single sign-on**.
1. Select **SAML** and the **Set up Single Sign-On with SAML** page will appear.

#### Basic SAML configuration for your app

1. To edit the basic SAML configuration options, select the **Edit** icon (a pencil) in the upper-right corner of the Basic SAML Configuration section.
1. Set **Identifier (Entity ID)** with an unique URL that follows the pattern, `http://{your-appName}.{your-domain}.com`. For instance: `http://webappsaml.contoso.com`. Copy the Entity ID value to be used in later steps.
1. Set **Reply URL** with the URL that Microsoft Entra will reply after the authentication. In this sample we are using `https://localhost:44347/`.
1. [Optional] Set the optional parameters if they are required in your scenario. The guide [Moving application authentication from Active Directory Federation Services to Microsoft Entra ID](https://learn.microsoft.com/azure/active-directory/manage-apps/migrate-adfs-apps-to-azure) is an excellent resource to learn about the various available options.

Learn more about [configuring SAML-based single sign-on in Microsoft Entra ID](https://learn.microsoft.com/azure/active-directory/manage-apps/configure-single-sign-on-non-gallery-applications).

#### Configure user attributes and claims

By default, the claims *givenname*, *surname*, *emailaddress* and *name* will be already configured for the application.

The web application registered on [chapter 1](https://github.com/Azure-Samples/ms-identity-dotnet-adfs-to-aad/tree/master/1-ADFS-Host/1-1-Setup-SAML-Playground/README.md) also uses **Employee-ID** claim, so we need to configure that as well. If you added extra claims on the AD FS registration of the web application, follow the steps below to register all the additional claims that should be returned by the Microsoft Entra SAML application:

1. In the **User Attributes and Claims** section, select the **Edit** icon (a pencil) in the upper-right corner.
1. Add additional claims that you would like to use in the web app project.
1. To add a claim, select **Add new claim** at the top of the page.
1. Enter the **Name**, for instance `employeeID`.
1. Enter the **Namespace** if desired, for instance `http://schemas.xmlsoap.org/ws/2005/05/identity/claims`.
1. Select the appropriate **Source** that contains the claim value. For **Employee-ID**, the source is **Attribute**.
1. Select the appropriate **Source Attribute** or **Transformation**, depending on what you selected on the previous step. For **Employee-ID**, select `user.employeeid`.
1. Select **Save**. The new claim appears in the table.

#### SAML Signing Certificate

1. In the **SAML Signing Certificate** section, if you don't have a certificate yet, select the **Edit** icon (a pencil) in the upper-right corner.
2. Select **New Certificate** and then **Save**.
3. Close the blade, refresh the page and copy the value for `App Federation Metadata Url` field. We will use it on the .NET MVC project.

### Customize Claims Emitted in Tokens

In Microsoft Entra, it is possible to customize claims emitted in tokens for specific applications. Check the [permitted values that can be emitted](https://learn.microsoft.com/entra/identity-platform/active-directory-claims-mapping#table-3-valid-id-values-per-source) and [SAML exceptions and restrictions](https://learn.microsoft.com/entra/identity-platform/active-directory-claims-mapping#exceptions-and-restrictions).

In order to customize the claims emitted in tokens, you have to create a **claim mapping policy** and assign it to the desired application. Learn [how to create and assign a claim mapping policy](https://learn.microsoft.com/entra/identity-platform/active-directory-claims-mapping#claims-mapping-policy-assignment).

## Configure the .NET MVC project (WebApp_SAML) to use your app registration

Open the project **WebApp_SAML** in your IDE (like Visual Studio) to configure the code.

1. Open the *Web.config* file.
1. Replace the value for `ida:ADFSMetadata` with the link that you copied from **App Federation Metadata Url** field.
1. Replace the value for `ida:Wtrealm` with the value that you set for **Identifier (Entity ID)**. For instance, `http://webappsaml.contoso.com`.
1. Save, clean and build the solution.

### Test the application

Clean and build the solution, then run the **WebApp_SAML** application and sign-in using an on-premise user (who already got synced) or a user from your Microsoft Entra tenant only.

All the claims configured on the **User Attributes and Claims** steps will be listed in the page, in case the signed-in user has a value set for it.

> If you find a bug in the sample, raise the issue on [GitHub Issues](../../issues).

> [Consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUODFCRVg4VTk2QUE2VEFPMUZKSEJNUFhWUyQlQCN0PWcu)

## We'd love your feedback!

Were we successful in addressing your learning objective? [Do consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUODFCRVg4VTk2QUE2VEFPMUZKSEJNUFhWUyQlQCN0PWcu)

We're always listening, and if you want to get in touch with you directly, send an email to <aadappfeedback@microsoft.com>.

## Next Step

- [Using on-prem Active Directory security groups in a Microsoft Entra application](../2-2-Security-Groups/README.md)

### Useful resources

- [Moving application authentication from AD FS to Microsoft Entra ID](https://learn.microsoft.com/azure/active-directory/manage-apps/migrate-adfs-apps-to-azure)
- [AD FS to Microsoft Entra app Migration Tool](https://github.com/AzureAD/Deployment-Plans/blob/master/ADFS%20to%20AzureAD%20App%20Migration/Readme.md)
- [Configure SAML-based single sign-on to non-gallery applications](https://learn.microsoft.com/azure/active-directory/manage-apps/configure-single-sign-on-non-gallery-applications)
