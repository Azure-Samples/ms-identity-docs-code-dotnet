using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Console_DeviceCodeFlow_MultiTarget
{
    internal class Program
    {
        private static PublicClientApplicationOptions appConfiguration = null;
        private static IConfiguration configuration;
        private static string MSGraphURL;
       
        // The MSAL Public client app
        private static IPublicClientApplication application;

        private static async Task Main(string[] args)
        {
            // Using appsettings.json to load the configuration settings
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            appConfiguration = configuration.Get<PublicClientApplicationOptions>();
            
            // We intend to obtain a token for Graph for the following scopes (permissions)
            string[] scopes = new[] { "user.read" };

            MSGraphURL = configuration.GetValue<string>("GraphApiUrl");

            // Sign-in user using MSAL and obtain an access token for MS Graph
            GraphServiceClient graphClient = await SignInAndInitializeGraphServiceClient(appConfiguration, scopes);

            // Call the /me endpoint of MS Graph
            await CallMSGraph(graphClient);
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
                                                    .WithDefaultRedirectUri()
                                                    .Build();

          
            AuthenticationResult result;

            try
            {
                var accounts = await application.GetAccountsAsync();
                // Try to acquire an access token from the cache. If device code is required, Exception will be thrown.
                result = await application.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                result = await application.AcquireTokenWithDeviceCode(scopes, deviceCodeResult =>
                   {
               // This will print the message on the console which tells the user where to go sign-in using
               // a separate browser and the code to enter once they sign in.
               // The AcquireTokenWithDeviceCode() method will poll the server after firing this
               // device code callback to look for the successful login of the user via that browser.
               // This background polling (whose interval and timeout data is also provided as fields in the
               // deviceCodeCallback class) will occur until:
               // * The user has successfully logged in via browser and entered the proper code
               // * The timeout specified by the server for the lifetime of this code (typically ~15 minutes) has been reached
               // * The developing application calls the Cancel() method on a CancellationToken sent into the method.
               //   If this occurs, an OperationCanceledException will be thrown (see catch below for more details).
               Console.WriteLine(deviceCodeResult.Message);
                       return Task.FromResult(0);
                   }).ExecuteAsync();
            }
            return result.AccessToken;
        }

        /// <summary>
        /// Sign in user using MSAL and obtain a token for MS Graph
        /// </summary>
        /// <returns></returns>
        private async static Task<GraphServiceClient> SignInAndInitializeGraphServiceClient(PublicClientApplicationOptions configuration, string[] scopes)
        {
            GraphServiceClient graphClient = new GraphServiceClient(MSGraphURL,
                new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", await SignInUserAndGetTokenUsingMSAL(configuration, scopes));
                }));

            return await Task.FromResult(graphClient);
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