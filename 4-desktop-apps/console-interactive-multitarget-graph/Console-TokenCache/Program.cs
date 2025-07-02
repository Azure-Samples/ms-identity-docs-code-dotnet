using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Console_TokenCache
{
    class Program
    {
        private static PublicClientApplicationOptions appConfiguration = null;
        private static IConfiguration configuration;
        private static string _authority;

        static async Task Main(string[] _)
        {
            // Using appsettings.json as our configuration settings
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            // Loading PublicClientApplicationOptions from the values set on appsettings.json
            appConfiguration = configuration
                .Get<PublicClientApplicationOptions>();

            // Building the AAD authority, https://login.microsoftonline.com/<tenant>
            _authority = string.Concat(appConfiguration.Instance, appConfiguration.TenantId);

            // Building a public client application
            var app = PublicClientApplicationBuilder.Create(appConfiguration.ClientId)
                                                    .WithAuthority(_authority)
                                                    .WithRedirectUri(appConfiguration.RedirectUri)
                                                    .Build();

            // Building StorageCreationProperties
            var storageProperties =
                 new StorageCreationPropertiesBuilder(CacheSettings.CacheFileName, CacheSettings.CacheDir, appConfiguration.ClientId)
                 .WithLinuxKeyring(
                     CacheSettings.LinuxKeyRingSchema,
                     CacheSettings.LinuxKeyRingCollection,
                     CacheSettings.LinuxKeyRingLabel,
                     CacheSettings.LinuxKeyRingAttr1,
                     CacheSettings.LinuxKeyRingAttr2)
                 .WithMacKeyChain(
                     CacheSettings.KeyChainServiceName,
                     CacheSettings.KeyChainAccountName)
                 .Build();

            // This hooks up the cross-platform cache into MSAL
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties);
            cacheHelper.RegisterCache(app.UserTokenCache);

            // Scope for Microsoft Graph
            string[] scopes = new[] { "user.read" };
            string graphApiUrl = configuration.GetValue<string>("GraphApiUrl");

            AuthenticationResult result;
            GraphServiceClient graphClient;
            User me;

            while (true)
            {
                // Display menu
                Console.WriteLine("------------ MENU ------------");
                Console.WriteLine("1. Acquire Token Silent / Interactive (not using embedded view)");
                Console.WriteLine("2. Acquire Token Silent / Interactive (using embedded view, currently not supported on .NET Core)");
                Console.WriteLine("3. Display Accounts (reads the cache)");
                Console.WriteLine("4. Clear cache");
                Console.WriteLine("x. Exit app");
                Console.Write("Enter your Selection:");
                char.TryParse(Console.ReadLine(), out var selection);

                try
                {
                    switch (selection)
                    {
                        case '1': // Silent / Interactive
                            Console.Clear();
                            Console.WriteLine("Acquiring token from the cache (silently), if it fails do it interactively");

                            result = await AcquireToken(app, scopes, false);

                            graphClient = GetGraphServiceClient(result.AccessToken, graphApiUrl);
                            me = await graphClient.Me.Request().GetAsync();

                            DisplayGraphResult(result, me);
                            break;

                        case '2': // Silent / Interactive with embedded view
                            Console.Clear();
                            Console.WriteLine("Acquiring token from the cache (silently), if it fails do it interactively using embedded view");

                            result = await AcquireToken(app, scopes, true);

                            graphClient = GetGraphServiceClient(result.AccessToken, graphApiUrl);
                            me = await graphClient.Me.Request().GetAsync();

                            DisplayGraphResult(result, me);
                            break;

                        case '3': // Display Accounts
                            Console.Clear();
                            var accounts2 = await app.GetAccountsAsync().ConfigureAwait(false);
                            if (!accounts2.Any())
                            {
                                Console.WriteLine("No accounts were found in the cache.");
                                Console.Write(Environment.NewLine);
                            }

                            foreach (var acc in accounts2)
                            {
                                Console.WriteLine($"Account for {acc.Username}");
                                Console.Write(Environment.NewLine);
                            }
                            break;

                        case '4': // Clear cache
                            Console.Clear();
                            var accounts3 = await app.GetAccountsAsync().ConfigureAwait(false);
                            foreach (var acc in accounts3)
                            {
                                Console.WriteLine($"Removing account for {acc.Username}");
                                Console.Write(Environment.NewLine);
                                await app.RemoveAsync(acc).ConfigureAwait(false);
                            }
                            break;

                        case 'x':
                            return;
                    }

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exception : " + ex);
                    Console.ResetColor();
                    Console.WriteLine("Hit Enter to continue");
                    Console.Read();
                    Console.Clear();
                }
            }
        }

        private static async Task<AuthenticationResult> AcquireToken(IPublicClientApplication app, string[] scopes, bool useEmbaddedView)
        {
            AuthenticationResult result;
            try
            {
                var accounts = await app.GetAccountsAsync();

                // Try to acquire an access token from the cache. If an interaction is required, 
                // MsalUiRequiredException will be thrown.
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                            .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // Acquiring an access token interactively. MSAL will cache it so we can use AcquireTokenSilent
                // on future calls.
                result = await app.AcquireTokenInteractive(scopes)
                            .WithUseEmbeddedWebView(useEmbaddedView)
                            .ExecuteAsync();
            }

            return result;
        }

        private static GraphServiceClient GetGraphServiceClient(string accessToken, string graphApiUrl)
        {
            GraphServiceClient graphServiceClient = new GraphServiceClient(graphApiUrl,
                                                                 new DelegateAuthenticationProvider(
                                                                     async (requestMessage) =>
                                                                     {
                                                                         await Task.Run(() =>
                                                                         {
                                                                             requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                                                                         });
                                                                     }));

            return graphServiceClient;
        }

        private static void DisplayGraphResult(AuthenticationResult result, User me)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(Environment.NewLine);
            Console.WriteLine($"Hello {result.Account.Username}");
            Console.Write(Environment.NewLine);
            Console.WriteLine("-------- GRAPH RESULT --------");
            Console.Write(Environment.NewLine);
            Console.WriteLine($"Id: {me.Id}");
            Console.WriteLine($"Display Name: {me.DisplayName}");
            Console.WriteLine($"Email: {me.Mail}");
            Console.Write(Environment.NewLine);
            Console.WriteLine("------------------------------");
            Console.Write(Environment.NewLine);
            Console.Write(Environment.NewLine);
            Console.ResetColor();

        }
    }
}
