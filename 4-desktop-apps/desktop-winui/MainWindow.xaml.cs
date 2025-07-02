using Microsoft.Identity.Client;
using Microsoft.UI.Xaml;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly HttpClient httpClient = new();

        // Generally, your MSAL client will have a lifecycle that matches the lifecycle
        // of the user's session in the application. In this sample, the lifecycle of the
        // MSAL client to the lifecycle of this form.
        private readonly IPublicClientApplication msalPublicClientApp;

        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "MSAL WinUI 3 Packaged Desktop App Sample";

            // Configure your public client application
            msalPublicClientApp = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(new PublicClientApplicationOptions
                {
                    // Enter the tenant ID obtained from the Microsoft Entra admin center
                    TenantId = "Enter the tenant ID obtained from the Microsoft Entra admin center",

                    // Enter the client ID obtained from the Microsoft Entra admin center
                    ClientId = "Enter the client ID obtained from the Microsoft Entra admin center"
                })
                .WithDefaultRedirectUri()
                .Build();
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
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
            graphCallResultTextBlock.Text = JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
            tokenAcquisitionTextBlock.Text = $"Access Token: {(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

            this.signInCallToActionTextBlock.Visibility = Visibility.Collapsed;
            this.authorizedPanel.Visibility = Visibility.Visible;
        }

        private async void SignOut_Click(object sender, RoutedEventArgs e)
        {
            // All cached tokens will be removed.
            // The next token request will require the user to sign in.
            foreach (var account in (await msalPublicClientApp.GetAccountsAsync()).ToList())
            {
                await msalPublicClientApp.RemoveAsync(account);
            }

            // Show the call to action and hide the results.
            graphCallResultTextBlock.Text = string.Empty;
            tokenAcquisitionTextBlock.Text = "";
            this.signInCallToActionTextBlock.Visibility = Visibility.Visible;
            this.authorizedPanel.Visibility = Visibility.Collapsed;
        }
    }
}