# Code samples for Microsoft identity platform documentation - .NET, C# 

Welcome to the Microsoft Identity Platform .NET Code Samples repository!

This repository provides a comprehensive set of end-to-end samples demonstrating how to integrate Microsoft Entra ID authentication and authorization into .NET applications. The samples are organized by scenario and technology, making it easy to find the right starting point for your needs.

## 📁 Repository Structure

The samples are grouped into five main categories, each corresponding to a common application scenario:

| Folder            | App description                                                                                  | Authentication libraries                      |
|-------------------|-------------------------------------------------------------------------------------------------|-----------------------------------------------|
| [1-web-apps](./1-web-apps)      | Interactive web applications built with ASP.NET Core (OIDC) and Blazor Server web apps with Microsoft Entra sign-in | Microsoft.Identity.Web<br>MSAL.NET        |
| [2-web-apis](./2-web-apis)      | Protected Web APIs, Azure Function APIs and APIs for On-Behalf-Of (OBO) flows              | Microsoft.Identity.Web<br>MSAL.NET        |
| [3-client-spa](./3-client-spa)  | Blazor WebAssembly SPA authentication                                                | Microsoft.Authentication.WebAssembly.Msal<br>MSAL.NET |
| [4-desktop-apps](./4-desktop-apps) | WinForms, WPF, WinUI interactive authentication apps, console device code/daemon apps and .NET MAUI cross-platform apps | MSAL.NET                                    |
| [5-migration](./5-migration)    | AD FS to Microsoft Entra ste-by-step migration guide                             | Microsoft.Identity.Web<br>MSAL.NET        |
| [6-advanced](./6-advanced)      | Access the logged-in user's token cache from background apps, APIs and services  | Microsoft.Identity.Web<br>MSAL.NET        |

---

Each folder contains one or more samples, each with its own README and instructions.

---

## 🚀 Getting Started

1. Browse the folders above to find the app type/scenario that matches your use case.
2. Read the sample’s README.md for prerequisites, setup instructions, and code walkthroughs.
3. Clone and run the sample following the provided steps.

All samples are built with the latest supported .NET versions and Microsoft.Identity libraries.

---

## 📝 Configuration & Security

- Configuration is managed via *appsettings.json* and environment variables.
- Secrets and credentials should never be committed to source control or exposed in GitHub. Ensure that you keep your credentials and details safe when using the material in these samples. Use [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/general/basic-concepts) or environment variables for sensitive data.

---

## 🤝 Contributing

We welcome contributions!  
Please see [*CONTRIBUTING.md*](CONTRIBUTING.md) for guidelines on submitting new samples or improvements.

---

## 📢 Support & Feedback

- For questions, open an [issue](https://github.com/Azure-Samples/ms-identity-docs-code-dotnet/issues).
- For Microsoft identity platform documentation, visit [learn.microsoft.com/entra/identity-platform](https://learn.microsoft.com/entra/identity-platform).

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

## 📚 Resources

- Documentation on learn.microsoft.com
  - [Microsoft identity platform product documentation](https://learn.microsoft.com/entra/identity-platform/)
  - [Microsoft Authentication Library (MSAL) documentation](https://learn.microsoft.com/entra/identity-platform/msal-overview)
- Authentication library source code on GitHub
  - [Microsoft Authentication Library for .NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet)
  - [Microsoft.Identity.Web](https://github.com/AzureAD/microsoft-identity-web)

---

Happy coding!