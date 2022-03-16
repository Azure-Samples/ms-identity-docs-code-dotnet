﻿using Microsoft.Identity.Client;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows;

namespace MsalExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient httpClient = new();

        // Generally, your MSAL client will have a lifecycle that matches the lifecycle
        // of the user's session in the application. In this sample, the lifecycle of the
        // MSAL client is tied to the lifecycle of this form.
        private readonly IPublicClientApplication msalPublicClientApp;

        public MainWindow()
        {
            InitializeComponent();

            // Configure your public client application
            msalPublicClientApp = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(new PublicClientApplicationOptions
                {
                    // 'Tenant ID' of your Azure AD instance - this value is a GUID
                    TenantId = "",

                    // 'Application (client) ID' of app registration in Azure portal - this value is a GUID
                    ClientId = ""
                })
                .WithDefaultRedirectUri() // http://localhost
                .Build();
        }

        /// <summary>
        /// Handle the "Sign In" button click. This will acquire an access token scoped to
        /// Microsoft Graph, either from the cache or from an interactive session. It will
        /// then use that access token in an HTTP request to Microsoft Graph and display
        /// the results.
        /// </summary>
        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult? msalAuthenticationResult = null;

            // Acquire a cached access token for Microsoft Graph if one is available from a prior
            // execution of this process.
            var accounts = await msalPublicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    // Will return a cached access token if available, refreshing if necessary.
                    msalAuthenticationResult = await msalPublicClientApp.AcquireTokenSilent(
                        new[] { "https://graph.microsoft.com/User.Read" },
                        accounts.First())
                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    // Nothing in cache for this account + scope, and interactive experience required.
                }
            }

            if (msalAuthenticationResult == null)
            {
                // This is likely the first authentication request in the application, so calling
                // this will launch the user's default browser and send them through a login flow.
                // After the flow is complete, the rest of this method will continue to execute.
                msalAuthenticationResult = await msalPublicClientApp.AcquireTokenInteractive(
                    new[] { "https://graph.microsoft.com/User.Read" })
                    .ExecuteAsync();
            }

            // Call Microsoft Graph using the access token acquired above.
            using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
            var graphResponseMessage = await httpClient.SendAsync(graphRequest);
            graphResponseMessage.EnsureSuccessStatusCode();

            // Present the results to the user (formatting the json for readability)
            using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
            GraphResultsTextBox.Text = JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
            AccessTokenSourceLabel.Content = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

            // Hide the call to action and show the results.
            SignInCallToActionTextBlock.Visibility = Visibility.Hidden;
            GraphResultsPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handle the "Sign Out" button click. This will remove all cached tokens from
        /// the MSAL client, resulting in any future usage requiring a reauthentication
        /// experience.
        /// </summary>
        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            // Signing out is removing all cached tokens, meaning the next token request will
            // require the user to sign in.
            foreach (var account in (await msalPublicClientApp.GetAccountsAsync()).ToList())
            {
                await msalPublicClientApp.RemoveAsync(account);
            }

            // Show the call to action and hide the results.
            GraphResultsPanel.Visibility = Visibility.Hidden;
            GraphResultsTextBox.Clear();
            SignInCallToActionTextBlock.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
