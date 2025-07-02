using Console_Interactive_CustomWebUI.CustomWebBrowser;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensibility;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Console_Interactive_CustomWebUI
{
    class Program
    {
        private static PublicClientApplicationOptions appConfiguration = null;
        private static IConfiguration configuration;
        private static string graphURL;

        // The MSAL Public client app
        private static IPublicClientApplication application;

        // Since the browser is started via Process.Start, there is no control over it,
        // So it is recommended to configure a timeout 
        private const int TimeoutWaitingForBrowserMs = 30 * 1000; //30 seconds

        static async Task Main(string[] args)
        {
            // Using appsettings.json as our configuration settings
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            appConfiguration = configuration
                .Get<PublicClientApplicationOptions>();

            string[] scopes = new[] { "user.read" };

            graphURL = configuration.GetValue<string>("GraphApiUrl");

            // Sign-in user using MSAL and obtain an access token for MS Graph
            GraphServiceClient graphClient = await SignInAndInitializeGraphServiceClient(appConfiguration, scopes);

            // Call the /me endpoint of MS Graph
            await CallMSGraph(graphClient);

            Console.ReadKey();
        }

        /// <summary>
        /// Sign in user using MSAL and obtain a token for MS Graph
        /// </summary>
        /// <returns></returns>
        private async static Task<GraphServiceClient> SignInAndInitializeGraphServiceClient(PublicClientApplicationOptions configuration, string[] scopes)
        {
            GraphServiceClient graphClient = new GraphServiceClient(graphURL,
                new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", await SignInUserAndGetTokenUsingMSAL(configuration, scopes));
                }));

            return await Task.FromResult(graphClient);
        }

        /// <summary>
        /// Signs in the user using the device code flow and obtains an Access token for MS Graph
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        private static async Task<string> SignInUserAndGetTokenUsingMSAL(PublicClientApplicationOptions configuration, string[] scopes)
        {
            // build the AAd authority Url
            string authority = string.Concat(configuration.Instance, configuration.TenantId);

            // Initialize the MSAL library by building a public client application
            application = PublicClientApplicationBuilder.Create(configuration.ClientId)
                                                    .WithAuthority(authority)
                                                    .WithRedirectUri(CustomBrowserWebUi.FindFreeLocalhostRedirectUri()) // required for CustomBrowserWebUi
                                                    .Build();


            AuthenticationResult result;

            try
            {
                var accounts = await application.GetAccountsAsync();

                // Try to acquire an access token from the cache, if UI interaction is required, MsalUiRequiredException will be thrown.
                result = await application.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // Acquiring an access token interactively using custom web UI.
                result = await application.AcquireTokenInteractive(scopes)
                            .WithCustomWebUi(new CustomBrowserWebUi()) //Using our custom web ui
                            .ExecuteAsync();
            }

            return result.AccessToken;
        }

        /// <summary>
        /// Call MS Graph and print results
        /// </summary>
        /// <param name="graphClient"></param>
        /// <returns></returns>
        private static async Task CallMSGraph(GraphServiceClient graphClient)
        {
            var me = await graphClient.Me.Request().GetAsync();

            // Printing the results
            Console.Write(Environment.NewLine);
            Console.WriteLine("-------- Data from call to MS Graph --------");
            Console.Write(Environment.NewLine);
            Console.WriteLine($"Id: {me.Id}");
            Console.WriteLine($"Display Name: {me.DisplayName}");
            Console.WriteLine($"Email: {me.Mail}");
        }
    }
}
