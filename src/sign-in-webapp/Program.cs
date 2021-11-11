// <ms_docref_import_types>
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
// </ms_docref_import_types>

// <ms_docref_add_msal>
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IEnumerable<string>? initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();
// </ms_docref_add_msal>

// <ms_docref_add_default_authz_policies>
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});
// </ms_docref_add_default_authz_policies>

// <ms_docref_add_default_controller_for_sign-in-out>
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();
// </ms_docref_add_default_controller_for_sign-in-out>

// <ms_docref_enable_authz_capabilities>
WebApplication app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// </ms_docref_enable_authz_capabilities>

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.Run();
