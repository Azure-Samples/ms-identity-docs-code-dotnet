using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace XPlat
{
    public partial class MainPage : ContentPage
    {
        private static readonly string s_clientId = "APPLICATION_(CLIENT)_ID";
        private static readonly string s_tenant = "TENANT_ID";
        private static readonly string s_authority = "https://login.microsoftonline.com/" + s_tenant;

        // The MSAL Public client app
        private static IPublicClientApplication s_publicClientApp;
        
        private static readonly HttpClient s_httpClient = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            try
            {
                // Initialize the MSAL library by building a public client application
                s_publicClientApp ??= PublicClientApplicationBuilder.Create(s_clientId)
                    .WithAuthority(s_authority)
#if ANDROID
                    .WithRedirectUri($"msal{s_clientId}://auth")
                    .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#else
                    .WithRedirectUri($"https://login.microsoftonline.com/common/oauth2/nativeclient")
#endif
                    .WithLogging((level, message, containsPii) =>
                    {
                        Debug.WriteLine($"MSAL: {level} {message} ");
                    }, LogLevel.Warning, enablePiiLogging: false, enableDefaultPlatformLogging: true)
                    .Build();

                // Sign-in user using MSAL and obtain an access token for MS Graph
                IEnumerable<IAccount> accounts = await s_publicClientApp.GetAccountsAsync().ConfigureAwait(false);
                IAccount firstAccount = accounts.FirstOrDefault();

                //Set the scope for API call to user.read
                var graphScope = new string[] { "user.read" };
                AuthenticationResult authResult;
                try
                {
                    // Signs in the user and obtains an Access token for MS Graph
                    authResult = await s_publicClientApp.AcquireTokenSilent(graphScope, firstAccount)
                                                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException ex)
                {
                    // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                    Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                    authResult = await s_publicClientApp.AcquireTokenInteractive(graphScope)
                                                        .ExecuteAsync()
                                                        .ConfigureAwait(false);
                }

                // Call the /me endpoint of Graph
                s_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                User graphUser = await s_httpClient.GetFromJsonAsync<User>("https://graph.microsoft.com/v1.0/me");

                TokenLabel.Text = "Display Name: " + graphUser.DisplayName + "\nBusiness Phone: " + graphUser.BusinessPhones.FirstOrDefault()
                                    + "\nGiven Name: " + graphUser.GivenName + "\nid: " + graphUser.Id
                                    + "\nUser Principal Name: " + graphUser.UserPrincipalName;
            }
            catch (MsalException msalEx)
            {
                TokenLabel.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalEx}";
            }

            SignInButton.IsVisible = false;
            SignOutButton.IsVisible = true;

            SemanticScreenReader.Announce(TokenLabel.Text);
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            IEnumerable<IAccount> accounts = await s_publicClientApp.GetAccountsAsync();
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                await s_publicClientApp.RemoveAsync(firstAccount);

                TokenLabel.Text = "User has signed-out";
            }
            catch (MsalException ex)
            {
                TokenLabel.Text = $"Error signing-out user: {ex.Message}";
            }

            SignInButton.IsVisible = true;
            SignOutButton.IsVisible = false;
        }
    }
}
