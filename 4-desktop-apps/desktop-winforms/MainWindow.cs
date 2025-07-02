using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MsalExample
{
    public partial class MainWindow : Form
    {
        private readonly HttpClient _httpClient = new();

        // In order to take advantage of token caching, your MSAL client singleton must
        // have a lifecycle that at least matches the lifecycle of the user's session in
        // the application. In this sample, the lifecycle of the MSAL client is tied to
        // the lifecycle of this form instance, which is the whole of the application.
        private readonly IPublicClientApplication msalPublicClientApp;

        public MainWindow()
        {
            InitializeComponent();

            // Configure your public client application
            msalPublicClientApp = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(new PublicClientApplicationOptions
                {
                    // Enter the tenant ID obtained from the Microsoft Entra admin center
                    TenantId = "Enter the client ID obtained from the Microsoft Entra admin center",

                    // Enter the client ID obtained from the Microsoft Entra admin center
                    ClientId = "Enter the tenant ID obtained from the Microsoft Entra admin center"
                })
                .WithDefaultRedirectUri() // http://localhost
                .Build();
        }

        // <summary>
        // Handle the "Sign In" button click. This will acquire an access token scoped to
        // Microsoft Graph, either from the cache or from an interactive session. It will
        // then use that access token in an HTTP request to Microsoft Graph and display
        // the results.
        // </summary>
        private async void SignInButton_Click(object sender, EventArgs e)
        {
            AuthenticationResult? msalAuthenticationResult = null;

            // Acquire a cached access token for Microsoft Graph if one is available from a prior
            // execution of this authentication flow.
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
                // This is likely the first authentication request since application start or after
                // Sign Out was clicked, so calling this will launch the user's default browser and
                // send them through a login flow. After the flow is complete, the rest of this method
                // will continue to execute.
                msalAuthenticationResult = await msalPublicClientApp.AcquireTokenInteractive(
                    new[] { "https://graph.microsoft.com/User.Read" })
                    .ExecuteAsync();
            }

            // Call Microsoft Graph using the access token acquired above.
            using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
            var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
            graphResponseMessage.EnsureSuccessStatusCode();

            // Present the results to the user (formatting the json for readability)
            using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
            GraphResultsTextBox.Text = JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
            AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

            // Hide the call to action and show the results.
            SignInCallToActionLabel.Hide();
            GraphResultsPanel.Show();
        }

        /// <summary>
        /// Handle the "Sign Out" button click. This will remove all cached tokens from
        /// the MSAL client, resulting in any future usage requiring a reauthentication
        /// experience.
        /// </summary>
        private async void SignOutButton_Click(object sender, EventArgs e)
        {
            // Signing out is removing all cached tokens, meaning the next token request will
            // require the user to sign in.
            foreach (var account in (await msalPublicClientApp.GetAccountsAsync()).ToList())
            {
                await msalPublicClientApp.RemoveAsync(account);
            }

            // Show the call to action and hide the results.
            GraphResultsPanel.Hide();
            GraphResultsTextBox.Clear();
            SignInCallToActionLabel.Show();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
