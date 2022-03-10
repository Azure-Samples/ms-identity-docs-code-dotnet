using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;

var config = new {
    // Full directory URL, in the form of https://login.microsoftonline.com/<tenant_id>
    Authority = "",
    // 'Application (client) ID' of app registration in Azure portal - this value is a GUID
    ClientId = "",
    // Client secret 'Value' (not its ID) from 'Client secrets' in app registration in Azure portal
    ClientSecret = "",
    // Client 'Object ID' of app registration in Azure portal - this value is a GUID
    ClientObjectId = ""
};

// This app instance should be a long-lived instance because
// it maintains the in-memory token cache.
IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();

app.AddInMemoryTokenCache();

AuthenticationResult msalAuthenticationResult = await app.AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();

// Get *this* application's application object from Microsoft Graph
var httpClient = new HttpClient();
using var graphRequest = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/applications/{config.ClientObjectId}");
graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
var graphResponseMessage = await httpClient.SendAsync(graphRequest);
graphResponseMessage.EnsureSuccessStatusCode();

using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
Console.WriteLine(JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
