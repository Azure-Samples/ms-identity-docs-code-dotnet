// <ms_docref_import_types>
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
// </ms_docref_import_types>

// <ms_docref_add_msal>
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches()
                .AddDownstreamWebApi("GraphApi", builder.Configuration.GetSection("DownstreamApi"));
builder.Services.AddAuthorization();
// </ms_docref_add_msal>

// <ms_docref_enable_authz_capabilities>
WebApplication app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// </ms_docref_enable_authz_capabilities>

// <ms_docref_protect_endpoint>
app.MapGet("/me", [Authorize()] async (IDownstreamWebApi downstreamWebApi) => await downstreamWebApi.CallWebApiForUserAsync("GraphApi"));
// </ms_docref_protect_endpoint>

app.Run();
