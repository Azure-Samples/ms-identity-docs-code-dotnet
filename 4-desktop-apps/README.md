# 🖥️ Desktop Samples

This folder contains .NET desktop samples demonstrating authentication and authorization scenarios using the Microsoft identity platform. These samples cover a variety of desktop technologies, including Console, Windows Forms, WPF, WinUI, and cross-platform MAUI.

## 📋 Samples Overview

| Folder Name                                                                     | Authentication Libraries Used                | .NET Version          |
|---------------------------------------------------------------------------------|----------------------------------------------|-----------------------|
| [console-cli](./console-cli)                                                    | Microsoft.Identity.Client (MSAL.NET) 4.64.0  | .NET 8.0              |
| [console-daemon](./console-daemon)                                              | Microsoft.Identity.Web 3.1                   | .NET 8.0              |
| [console-device-code-flow](./console-device-code-flow)                          | Microsoft.Identity.Client (MSAL.NET) 4.13.0  | netcoreapp3.1; net472 | 
| [console-html-browser](./console-html-browser)                                  | Microsoft.Identity.Client (MSAL.NET) 4.9.0   | netcoreapp3.1; net472 |
| [console-interactive-multitarget-graph](./console-interactive-multitarget-graph)| Microsoft.Identity.Client (MSAL.NET)         | netcoreapp3.1; net472 |
| [console-web-browser](./console-web-browser/)                                   | Microsoft.Identity.Client (MSAL.NET) 4.9.0   | netcoreapp3.1 |
| [desktop-winforms](./desktop-winforms)                                          | Microsoft.Identity.Client (MSAL.NET) 4.x     | .NET 8.0     |
| [desktop-wpf](./desktop-wpf)                                                    | Microsoft.Identity.Client (MSAL.NET) 4.x     | .NET 8.0     |
| [desktop-winui](./desktop-winui)                                                | Microsoft.Identity.Client (MSAL.NET) 4.x     | .NET 8.0     |
| [xplat-maui](./xplat-maui)                                                      | Microsoft.Identity.Client (MSAL.NET) 4.38.0  | .NET 6.0     |

> [!NOTE]  
> All samples use the latest supported versions of the Microsoft identity libraries and are configured for secure, modern authentication scenarios.

---

### ☑️ Prerequisites

You will need the following to run any of these samples

   - [.NET](https://dotnet.microsoft.com/download/dotnet/8.0)
   - A Microsoft Entra tenant and app registration (see each sample's README for details)
   - An editor or IDE such as [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### 📥 Clone the Repository

1. Navigate to where you want to have the sample located, and enter the following

    ```sh
    git clone https://github.com/MicrosoftDocs/ms-identity-docs-code-dotnet.git
    ```
2. Navigate to the desktop app folder in the sample you have downloaded by using the following command;

    ```sh
    cd ms-identity-docs-code-dotnet/4-desktop-apps/
    ```
---

## 📚 Resources

- [Microsoft Identity Platform Documentation](https://learn.microsoft.com/entra/identity-platform/)
- [Microsoft.Identity.Client (MSAL.NET) Library](https://learn.microsoft.com/entra/identity-platform/msal-overview)
- [Microsoft Entra App Registration](https://learn.microsoft.com/entra/identity-platform/quickstart-register-app)
- [Securing Desktop Apps with Microsoft Identity](https://learn.microsoft.com/entra/identity-platform/tutorial-v2-windows-desktop)
- [Microsoft .NET Desktop Documentation](https://learn.microsoft.com/dotnet/desktop/)

---