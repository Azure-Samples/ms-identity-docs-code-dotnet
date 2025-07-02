# 🌐 Web App Samples

This folder contains a collection of .NET web application samples demonstrating authentication and authorization scenarios using the Microsoft identity platform. Each sample showcases a different web application type or authentication flow, using up-to-date libraries and best practices.

## 📋 Samples Overview

| 📁 Folder Name                                     | 🔑 Authentication Libraries Used                             | 🏷️ .NET Version  |
|-----------------------------------------------------|--------------------------------------------------------------|------------------|
| [web-app-aspnet-core](./web-app-aspnetcore)                  | Microsoft.Identity.Web, Microsoft.Identity.Client (MSAL.NET) | .NET 8.0         |
| [web-app-blazor-server](./web-app-blazor-server)    | Microsoft.Identity.Web, Microsoft.Identity.Client (MSAL.NET) | .NET 8.0         |

> [!NOTE]
> All samples use the latest supported versions of the Microsoft identity libraries and are configured for secure, modern authentication scenarios.

---

## 🚀 Getting Started

Follow these steps to set up your environment and run any of the web app samples in this folder.

### ☑️ Prerequisites

You will need the following to run any of these samples:

   - [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - A Microsoft Entra tenant and app registration (see each sample's README for details)
   - An editor or IDE such as [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### 📥 Clone the Repository

1. Navigate to where you want to have the sample located, and enter the following:

    ```sh
    git clone https://github.com/your-org/ms-identity-docs-code-dotnet.git
    ```

2. Navigate to the web app folder in the sample you have downloaded by using the following command;

    ```sh
    cd ms-identity-docs-code-dotnet/1-web-apps
    ```

---

## 📚 Resources

- [Microsoft Identity Platform Documentation](https://learn.microsoft.com/entra/identity-platform/)
- [Microsoft.Identity.Web Library](https://learn.microsoft.com/entra/msal/dotnet/microsoft-identity-web/)
- [MSAL.NET Library](https://learn.microsoft.com/entra/identity-platform/msal-overview)
- [Microsoft Entra App Registration](https://learn.microsoft.com/entra/identity-platform/quickstart-register-app)
- [Securing ASP.NET Core with Microsoft Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity)

---