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
                .AddDownstreamWebApi("GraphApi", builder.Configuration.GetSection("GraphApi"));
// </ms_docref_add_msal>

WebApplication app = builder.Build();

// <ms_docref_protect_endpoint>
app.MapGet("/application", async (IDownstreamWebApi downstreamWebApi) => await downstreamWebApi.CallWebApiForAppAsync("GraphApi"));
// </ms_docref_protect_endpoint>

app.Run();
