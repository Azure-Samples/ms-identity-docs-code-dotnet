using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text.Json;

var config = new PublicClientApplicationOptions
{
    // 'Directory (tenant) ID' of app registration in the Azure portal - this value is a GUID
    TenantId = "",

    // 'Application (client) ID' of app registration in Azure portal - this value is a GUID
    ClientId = ""
};

// In order to take advantage of token caching, your MSAL client singleton must
// have a lifecycle that at least matches the lifecycle of the user's session in
// the console application.
IPublicClientApplication publicMsalClient = PublicClientApplicationBuilder.CreateWithApplicationOptions(config)
                                                                          .Build();

// Initiate the device code flow. If access token acquisition needs to happen multiple
// times in the console application, only call this after checking for a cached token via
// a call to publicMsalClient.AcquireTokenSilent(...).
AuthenticationResult msalAuthenticationResult = await publicMsalClient.AcquireTokenWithDeviceCode(
    new[] { "https://graph.microsoft.com/User.Read" }, deviceCodeResultCallback =>
{
    // This will print the message on the console which tells the user where to go sign-in using
    // a separate browser and the code to enter once they sign in.
    // The AcquireTokenWithDeviceCode() method will poll the server after firing this
    // device code callback to look for the successful login of the user via that browser.
    Console.WriteLine(deviceCodeResultCallback.Message);
    return Task.CompletedTask;
}).ExecuteAsync();

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