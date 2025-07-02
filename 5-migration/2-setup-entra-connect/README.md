# Configure Microsoft Entra Connect to synchronize with a Microsoft Entra tenant

## Scenario

This page lists the steps to ensure that relevant bits of information are being synchronized from the on-premise Active directory to its connected Microsoft Entra tenant.

## About the page

In most organizations, the AD Connect sync tool should be already installed and configured. So we advise you follow the installation steps here only on a test instance of an Active Directory domain environment.

If it is already installed, skip to [Post-Installation steps](#post-installation-steps).

### Prerequisites

- An on-premise Active Directory environment
- A Microsoft Entra tenant. For more information on how to get a Microsoft Entra tenant, see [How to get a Microsoft Entra tenant](https://learn.microsoft.com/entra/identity-platform/quickstart-create-new-tenant)
- A tenant admin account in Microsoft Entra ID

The [Microsoft Entra Connect tool](https://www.microsoft.com/download/details.aspx?id=47594) will guide you through the initial steps on how to synchronize an AD FS environment with a Microsoft Entra tenant.

## Before you install Microsoft Entra Connect

Read the [Microsoft Entra Connect prerequisites](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-install-prerequisites) before installing it. This documentation contains information about hardware and software requirements, security guidelines, installation best practices, and operations not supported by the tool.

## Installing Microsoft Entra Connect

On a domain-joined Windows Server 2012 or later, [download and install Microsoft Entra Connect](https://www.microsoft.com/download/details.aspx?id=47594). It is highly recommended that this server is a domain controller.

The installation wizard will walk you through the tool configuration.

You will be asked to provide details about the Microsoft Entra tenant where you would like to migrate. A tenant admin account is required for this step.

## Post-Installation steps

After installation and configuration is complete, Microsoft Entra Connect will automatically run and synchronize **users** and **groups** from the on-premise Active Directory environment to your Microsoft Entra tenant.

> Please wait this operation to finish before executing extra steps. You should be able to confirm that synchronization succeeded by signing-into the Azure portal and check if all users and groups are available in the Microsoft Entra tenant.

Use [Graph Explorer](https://aka.ms/ge) to carefully examine the attributes of **users** and **groups** to ensure that all required attributes were synced.

## Additional sync tasks

If you run Microsoft Entra Connect installation wizard again, it offers options for maintenance and to set additional tasks.

See [additional tasks available](https://learn.microsoft.com/azure/active-directory/hybrid/how-to-connect-installation-wizard) for details about extra configuration steps.

## Next Step

- [Optional] If your on premise Active Directory have **Directory Extensions** that you'd like to synchronize to the Microsoft Entra tenant, [move to the next chapter to learn how to migrate them](../1-3-Directory-Extensions/README.md).
- [Migrate this .NET MVC application to a Microsoft Entra tenant](./../2-AAD-Migration/2-1-SAML-WebApp/README.md)

## Useful resources

- Learn how to [migrate the web app SAML application to Microsoft Entra ID](https://learn.microsoft.com/azure/active-directory/manage-apps/migrate-adfs-apps-to-azure).
