using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text.Json;

var config = new PublicClientApplicationOptions
{
    // 'Directory (tenant) ID' of the app registration in the Microsoft Entra admin center
    TenantId = "Enter the client ID obtained from the Microsoft Entra admin center",

    // 'Application (client) ID' of the app registration in the Microsoft Entra admin center
    ClientId = "Enter the tenant ID obtained from the Microsoft Entra admin center"
};

// In order to take advantage of token caching, your MSAL client singleton must
// have a lifecycle that at least matches the lifecycle of the user's session in
// the console application.
IPublicClientApplication publicMsalClient = PublicClientApplicationBuilder.CreateWithApplicationOptions(config)
                                                                          .Build();

AuthenticationResult? msalAuthenticationResult = null;

// Attempt to use a cached access token if one is available. This will renew existing, but
// expired access tokens if possible. In this specific sample, this will always result in
// a cache miss, but this pattern would be what you'd use on subsequent calls that require
// the usage of the same access token.
IEnumerable<IAccount> accounts = (await publicMsalClient.GetAccountsAsync()).ToList();

if (accounts.Any())
{
    try
    {
        msalAuthenticationResult = await publicMsalClient.AcquireTokenSilent(
            new[] { "https://graph.microsoft.com/User.Read" },
            accounts.First()).ExecuteAsync();
    }
    catch (MsalUiRequiredException)
    {
        // No usable cached token was found for this scope + account or Entra ID insists in
        // an interactive user flow.
    }
}

if (msalAuthenticationResult == null)
{
    // Initiate the device code flow.
    msalAuthenticationResult = await publicMsalClient.AcquireTokenWithDeviceCode(
        new[] { "https://graph.microsoft.com/User.Read" }, deviceCodeResultCallback =>
    {
        // This will print the message on the console which tells the user where to go sign-in using
        // a separate browser and the code to enter once they sign in.
        // The AcquireTokenWithDeviceCode() method will poll the server after firing this
        // device code callback to look for the successful login of the user via that browser.
        Console.WriteLine(deviceCodeResultCallback.Message);
        return Task.CompletedTask;
    }).ExecuteAsync();
}

// At this point we now have a valid access token for Microsoft Graph, with only the specific scopes
// necessary to complete the following call. Build the Microsoft Graph HTTP request, using the obtained
// access token.
using var graphHttpRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
graphHttpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);

// Make the API call to Microsoft Graph
var httpClient = new HttpClient();
HttpResponseMessage graphHttpResponse = await httpClient.SendAsync(graphHttpRequest);
graphHttpResponse.EnsureSuccessStatusCode();

// Present the results to the user (formatting the JSON for readability)
var graphResponseBody = JsonDocument.Parse(await graphHttpResponse.Content.ReadAsStringAsync());
Console.WriteLine(JsonSerializer.Serialize(graphResponseBody,
    new JsonSerializerOptions()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    }));