# Protected ASP.NET Core minimal API | Microsoft identity platform

The sample code provided here has been created using minimal web API in ASP.NET Core 6.0.

:link: To know more about how this sample has been generated, please visit https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code

## Prerequisites

1. Download .NET 6.0 SDK https://dotnet.microsoft.com/download/dotnet/6.0

## Scaffold

1. execute the following command to create the new web api project

   ```bash
   dotnet new webapi -minimal -o <name>
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

