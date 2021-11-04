# Protected ASP.NET Core minimal API | Microsoft identity platform

The sample code provided here has been created using minimal web API in ASP.NET Core 6.0, and slightly modified to be protected for a single organization using [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0) that interacts with [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview).  In other words, a very minimalist web api is secured by adding an authorization layer before user requests can reach protected resources.  At this point it is expected that the user authentication had already happened, so a token containing user's information is being sent in the request headers to be used in the authorization process.

:link: For more information about how to proctect your projects, please let's take a look at https://docs.microsoft.com/en-us/azure/active-directory/develop/sample-v2-code. To know more about how this sample has been generated, please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code

## Prerequisites

1. Download .NET 6.0 SDK https://dotnet.microsoft.com/download/dotnet/6.0

## Scaffold

1. execute the following command to create the new web api project

   ```bash
   dotnet new webapi -minimal -o <name>
   ```

1. Add donet package for ASP.NET Core Identity

   ```bash
   dotnet add package Microsoft.Identity.Web
   ```

## Run the web API

1. execute the following command to get the app up and running:

   ```bash
   dotnet run
   ```

## Send request to the web API

1. once the app is listening, execute the following to send the a request.

   ```bash
   curl -X GET https://localhost:7188/weatherforecast -k
   ```

