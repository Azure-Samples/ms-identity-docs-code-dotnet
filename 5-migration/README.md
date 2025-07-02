---
page_type: sample
languages:
- csharp
products:
- entra-id
- dotnet
description: "Guidance to help migrate web applications from AD FS to Microsoft Entra"
urlFragment: "ms-identity-dotnet-adfs-to-aad"
---

# 🔄 AD FS to Microsoft Entra application migration playbook for developers

These ASP.NET code samples and accompanying tutorials will help you learn how to safely and securely migrate your applications integrated with Active Directory Federation Services (AD FS) to Microsoft Entra ID. This sample is aimed at developers who not only need to learn configuring apps on both AD FS and Microsoft Entra, but also become aware and confident of changes their code base will require in this process.

Each folder in this repo are arranged as *steps*, each with a README and sample code for a scenario. These cover the most common types of authentication utilized by applications integrated with AD FS today, and we hope they let you learn and gain valuable experience before undertaking an app migration initiative for your applications currently running in production.

## ☑️ Prerequisites

- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework)
- [AD FS](https://learn.microsoft.com/windows-server/identity/ad-fs/ad-fs-overview) environment
- [Microsoft Entra tenant](https://learn.microsoft.com/entra/identity-platform/quickstart-create-new-tenant)

## 🔄 Migration steps

By following the steps in this code sample, you;

- Integrate the provided sample web application (using the [SAML](https://learn.microsoft.com/entra/identity-platform/single-sign-on-saml-protocol) protocol for authentication) with an AD FS instance. 
- Migrate this application to a Microsoft Entra tenant
- Learn how to change the authentication protocol from SAML to [OAuth 2.0 and OpenID Connect](https://learn.microsoft.com/entra/identity-platform/v2-protocols), allowing you to reap the benefits of accessing rich APIs like [Microsoft Graph](https://learn.microsoft.com/graph/overview) and the [Azure REST API](https://learn.microsoft.com/rest/api/azure/) amongst others.

We have also covered the following topics in some detcail as they might play a big part in your app migration work:

- Configuring [Microsoft Entra Connect sync](https://learn.microsoft.com/entra/identity/hybrid/connect/how-to-connect-sync-whatis).
- Migrating [Directory Extensions](https://learn.microsoft.com/entra/identity/hybrid/connect/how-to-connect-sync-feature-directory-extensions) to Microsoft Entra.
- Sync and use Security Groups on the migrated application.

## 📋 Sample Overview

| ☑️ Steps                                            | 📝 Description                              |
|--------------------------------------------------|-------------------------------|
| [1. Integrate app with AD FS](./1-saml-playground/README.md) | Register a .NET MVC Web application on on-prem AD with an AD FS instance |
| [2. Set up Microsoft Entra Connect](./2-setup-entra-connect/README.md) | A brief look at the Microsoft Entra Connect configuration that synchronizes on=premise data to a Microsoft Entra tenant. |
| [3. Directory Extensions](./3-migrate-directory-extensions/README.md) | Migrate Directory Extensions from on-prem Active Directory to your Microsoft Entra tenant. |
| [4. Integrate app with Microsoft Entra](./4-saml-webapp/README.md)| Migrating the SAML ASP.NET MVC web app from AD FS to Microsoft Entra.|
| [5. Using Security groups](./5-security-groups/README.md) | Using on-premise Active Directory security groups in applications migrated to a Microsoft Entra tenant. |
| [6. Convert from SAML to OIDC](./6-saml-to-oidc/README.md) | Migrate a SAML ASP.NET application to use [OAuth 2.0 and OpenID Connect](https://learn.microsoft.com/entra/identity-platform/v2-protocols).|

## 💬 Community Help and Support

Use [Stack Overflow](http://stackoverflow.com/questions/tagged/msal) to get support from the community.

If you find a bug in the sample, raise the issue on [GitHub Issues](../issues).

To provide feedback on or suggest features for Microsoft Entra ID, visit [User Voice](https://feedback.azure.com/forums/169401-azure-active-directory).

## 🤝 Contributing

Were we successful in addressing your learning objective? [Do consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUODFCRVg4VTk2QUE2VEFPMUZKSEJNUFhWUyQlQCN0PWcu)

We're always listening, and if you want to get in touch with you directly, send an email to <aadappfeedback@microsoft.com>.

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## 📚 Resources

- [Microsoft Identity Platform Documentation](https://learn.microsoft.com/entra/identity-platform/)
- [Moving application authentication from AD FS to Microsoft Entra](https://learn.microsoft.com/entra/identity/enterprise-apps/migrate-adfs-apps-stages)