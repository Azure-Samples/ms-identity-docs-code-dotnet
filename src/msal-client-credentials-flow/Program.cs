using System.Text.Json;
// <ms_docref_import_types>
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
// </ms_docref_import_types>

// <ms_docref_add_msal>
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Acquire a token from Azure AD for this client to access Microsoft Graph based
// on the permissions granted this application in its Azure AD App registration.
// The client credential flow will automatically attempt to use or renew any cached
// tokens, without the need to call acquireTokenSilently first.
// It is possible to read from the stdout log entries that the requested access token (AT) is being cached.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches()
                .AddDownstreamWebApi("GraphApi", builder.Configuration.GetSection("GraphApi"));
// </ms_docref_add_msal>

WebApplication app = builder.Build();

// <ms_docref_protect_endpoint>
app.MapGet("api/application", async (IDownstreamWebApi downstreamWebApi) =>
    {
      using var response = await downstreamWebApi.CallWebApiForAppAsync("GraphApi").ConfigureAwait(false);

      if (response.StatusCode == System.Net.HttpStatusCode.OK)
      {
        var graphApiResponse = await response.Content.ReadFromJsonAsync<JsonDocument>().ConfigureAwait(false);
        return JsonSerializer.Serialize(graphApiResponse, new JsonSerializerOptions { WriteIndented = true });
      }
      else
      {
        var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}: {error}");
      }
    });
// </ms_docref_protect_endpoint>

app.Run();
