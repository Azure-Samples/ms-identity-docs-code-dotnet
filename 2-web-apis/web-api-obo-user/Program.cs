using System.Text.Json;
// <ms_docref_import_types>
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;

// </ms_docref_import_types>

// <ms_docref_add_msal>
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi()
// The access token came from the in-memory token cache, which maintains
// the on-behalf-of access token, per user-assertion, based on the provided access
// token to this API.
                .AddInMemoryTokenCaches()
                .AddDownstreamApi("GraphApi", builder.Configuration.GetSection("GraphApi"));
builder.Services.AddAuthorization();
// </ms_docref_add_msal>

// <ms_docref_enable_authz_capabilities>
WebApplication app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// </ms_docref_enable_authz_capabilities>

// <ms_docref_protect_endpoint>
app.MapGet("/api/me", [Authorize()] async (IDownstreamApi downstreamWebApi) =>
{
  var response = await downstreamWebApi.CallApiForUserAsync("GraphApi").ConfigureAwait(false);

  var graphApiResponse = await response.Content.ReadFromJsonAsync<JsonDocument>().ConfigureAwait(false);
  return JsonSerializer.Serialize(graphApiResponse, new JsonSerializerOptions { WriteIndented = true });
});
// </ms_docref_protect_endpoint>

app.Run();
