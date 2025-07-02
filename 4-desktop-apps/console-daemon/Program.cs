using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;

var config = new {
    // Full directory URL, in the form of https://login.microsoftonline.com/<tenant_id>
    Authority = " https://login.microsoftonline.com/Enter the tenant ID obtained from the Microsoft Entra admin center",
    // Enter the client ID obtained from the Microsoft Entra admin center
    ClientId = "Enter the client ID obtained from the Microsoft Entra admin center",
    // Client secret 'Value' (not its ID) from 'Client secrets' in the Microsoft Entra admin center
    ClientSecret = "Enter the client secret value obtained from the Microsoft Entra admin center",
    // Client 'Object ID' of app registration in Microsoft Entra admin center - this value is a GUID
    ClientObjectId = "Enter the client Object ID obtained from the Microsoft Entra admin center"
};

// This app instance should be a long-lived instance because
// it maintains the in-memory token cache.
IConfidentialClientApplication msalClient = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();

msalClient.AddInMemoryTokenCache();

AuthenticationResult msalAuthenticationResult = await msalClient.AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();

// Get *this* application's application object from Microsoft Graph
var httpClient = new HttpClient();
using var graphRequest = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/applications/{config.ClientObjectId}");
graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
var graphResponseMessage = await httpClient.SendAsync(graphRequest);
graphResponseMessage.EnsureSuccessStatusCode();

using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
Console.WriteLine(JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
