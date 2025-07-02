using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
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

        // Object with the custom HTML 
        private static SystemWebViewOptions _customWebView = GetCustomHTML();

        static async Task Main(string[] args)
        {
            // Using appsettings.json as our configuration settings
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            appConfiguration = configuration
                .Get<PublicClientApplicationOptions>();

            // We intend to obtain a token for Graph for the following scopes (permissions)
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
                                                    .WithRedirectUri(configuration.RedirectUri)
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
                // Acquiring an access token interactively using the custom html.
                result = await application.AcquireTokenInteractive(scopes)
                            .WithSystemWebViewOptions(_customWebView) // Using the custom html
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

        /// <summary>
        /// Returns a custom HTML for the authorization success or failure, and redirect url. 
        /// For more available options, please inspect the SystemWebViewOptions class.
        /// </summary>
        /// <returns></returns>
        private static SystemWebViewOptions GetCustomHTML()
        {
            return new SystemWebViewOptions
            {
                HtmlMessageSuccess = @"<html style='font-family: sans-serif;'>
                                      <head><title>Authentication Complete</title></head>
                                      <body style='text-align: center;'>
                                          <header>
                                              <h1>Custom Web UI</h1>
                                          </header>
                                          <main style='border: 1px solid lightgrey; margin: auto; width: 600px; padding-bottom: 15px;'>
                                              <h2 style='color: limegreen;'>Authentication complete</h2>
                                              <div>You can return to the application. Feel free to close this browser tab.</div>
                                          </main>
    
                                      </body>
                                    </html>",

                HtmlMessageError = @"<html style='font-family: sans-serif;'>
                                  <head><title>Authentication Failed</title></head>
                                  <body style='text-align: center;'>
                                      <header>
                                          <h1>Custom Web UI</h1>
                                      </header>
                                      <main style='border: 1px solid lightgrey; margin: auto; width: 600px; padding-bottom: 15px;'>
                                          <h2 style='color: salmon;'>Authentication failed</h2>
                                          <div><b>Error details:</b> error {0} error_description: {1}</div>
                                          <br>
                                          <div>You can return to the application. Feel free to close this browser tab.</div>
                                      </main>
    
                                  </body>
                                </html>"
            };
        }
    }
}
