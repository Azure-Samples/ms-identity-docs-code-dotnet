using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace XPlat
{
    public partial class MainPage : ContentPage
    {
        //Set the scope for API call to user.read
        private string[] scopes = new string[] { "user.read" };
        private const string ClientId = "APPLICATION_(CLIENT)_ID";
        private const string Tenant = "TENANT_ID";
        private const string Authority = "https://login.microsoftonline.com/" + Tenant;

        // The MSAL Public client app
        private static IPublicClientApplication PublicClientApp;

        private static string MSGraphURL = "https://graph.microsoft.com/v1.0/";
        private static AuthenticationResult AuthResult;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            try
            {
                // Initialize the MSAL library by building a public client application
                PublicClientApp ??= PublicClientApplicationBuilder.Create(ClientId)
                    .WithAuthority(Authority)
                    .WithRedirectUri($"https://login.microsoftonline.com/common/oauth2/nativeclient")
                    .WithLogging((level, message, containsPii) =>
                    {
                        Debug.WriteLine($"MSAL: {level} {message} ");
                    }, LogLevel.Warning, enablePiiLogging: false, enableDefaultPlatformLogging: true)
                    .Build();

                // Sign-in user using MSAL and obtain an access token for MS Graph
                GraphServiceClient graphClient = new GraphServiceClient(MSGraphURL,
                    new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
                        IAccount firstAccount = accounts.FirstOrDefault();

                        try
                        {
                            // Signs in the user and obtains an Access token for MS Graph
                            AuthResult = await PublicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                                              .ExecuteAsync();
                        }
                        catch (MsalUiRequiredException ex)
                        {
                            // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                            Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                            AuthResult = await PublicClientApp.AcquireTokenInteractive(scopes)
                                                              .ExecuteAsync()
                                                              .ConfigureAwait(false);
                        }

                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", AuthResult.AccessToken);
                    }));

                // Call the /me endpoint of Graph
                User graphUser = await graphClient.Me.Request().GetAsync();

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
            IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync();
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                await PublicClientApp.RemoveAsync(firstAccount);

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
