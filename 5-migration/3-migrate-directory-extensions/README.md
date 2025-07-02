# Migrating Directory Extensions from on-prem Active Directory to your Microsoft Entra tenant

If you've created **Directory Extensions** in your on-premise Active Directory entities and you wish to synchronize them to your Microsoft Entra tenant, this document provides the necessary guidance to help you do so.

This document, [Microsoft Entra Connect](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-sync-whatis) also guides you how to configure Microsoft Entra Connect to synchronize Directory Extensions.

### Prerequisites

- An on-premise Active Directory environment
- A Microsoft Entra tenant. For more information on how to get a Microsoft Entra tenant, see [How to get a Microsoft Entra tenant](https://learn.microsoft.com/entra/identity-platform/quickstart-create-new-tenant)
- [Microsoft Entra Connect](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-sync-whatis) configured on a domain-joined machine

## Migrate Directory Extensions

While **users** and **groups**  will get synced to Microsoft Entra tenant using the Microsoft Entra Connect tool out of the box, **Directory Extensions** require a few extra steps to get it synced.

>If your on-premise Active Directory doesn't have Directory Extensions but you would like to add one to try this sample, please follow [this tutorial to create an extension](https://social.technet.microsoft.com/wiki/contents/articles/51121.active-directory-how-to-add-custom-attribute-to-schema.aspx).

## Configuring Microsoft Entra Connect for Directory Extensions

1. Open the Microsoft Entra Connect tool and select **Configure**
2. Select the option **Customize synchronization options** and click **Next**
3. Sign-in with a Microsoft Entra global administrator user account
4. Select **Next** on the *Connect your directories* tab
5. Select **Next** on the *Domain and OU filtering* tab
6. Check the box **Directory extension sync** and click **Next**
7. The following screen will show all the available attributes in your on-prem Active Directory. Move to the right box all the attributes that you would like to send to the Microsoft Entra tenant
8. Click **Next** and the synchronization will be executed.

Learn more details about [synchronizing Directory Extensions to Microsoft Entra tenant](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-sync-feature-directory-extensions).

### Test the Directory Extensions migration

To test the migration, use the [Graph Explorer](https://aka.ms/ge) and sign-in using a user account that has some of the extra attributes set.

Once signed-in and after consenting, copy and paste the URL, `https://graph.microsoft.com/beta/me`, and the extra attributes will be presented in the result.

>Note: When you sync Directory Extensions via Microsoft Entra Connect tool, their name on Microsoft Graph will have an auto-generated prefix that **cannot be edited**. For instance, if you have an extension called `regionDivision` and use the tool so sync it with Microsoft Entra, its name will be transformed to something like: `extension_90f5761cbd854b259d47fde20b522087_regionDivision`. [Learn more about it here](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-sync-feature-directory-extensions#configuration-changes-in-azure-ad-made-by-the-wizard).

> If you find a bug in the sample, raise the issue on [GitHub Issues](../../issues).

## Use Directory Extensions in Dynamic Groups

One of the most useful scenario in dynamic groups is the usage of Directory Extensions to dynamically associate users to it.

If you would like to configure dynamic groups, [follow this tutorial](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-sync-feature-directory-extensions#use-the-attributes-in-dynamic-groups).

## We'd love your feedback!

Were we successful in addressing your learning objective? [Do consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUODFCRVg4VTk2QUE2VEFPMUZKSEJNUFhWUyQlQCN0PWcu)

We're always listening, and if you want to get in touch with you directly, send an email to <aadappfeedback@microsoft.com>.

## Next Step

- [Migrate this .NET MVC application to a Microsoft Entra tenant](./../2-AAD-Migration/2-1-SAML-WebApp/README.md)

### Useful resources

- [Moving application authentication from AD FS to Microsoft Entra ID](https://learn.microsoft.com/azure/active-directory/manage-apps/migrate-adfs-apps-to-azure)
- [AD FS to Microsoft Entra app Migration Tool](https://github.com/AzureAD/Deployment-Plans/blob/master/ADFS%20to%20AzureAD%20App%20Migration/Readme.md)
- [Configure SAML-based single sign-on to non-gallery applications](https://learn.microsoft.com/azure/active-directory/manage-apps/configure-single-sign-on-non-gallery-applications)
- [Synchronizing Directory Extensions to Microsoft Entra](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-sync-feature-directory-extensions)
- [Using Directory Extensions in Dynamic groups](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-sync-feature-directory-extensions#use-the-attributes-in-dynamic-groups)
