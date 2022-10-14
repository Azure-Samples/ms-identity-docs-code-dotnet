// <ms_docref_import_types>
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using System.Security.Claims;
// </ms_docref_import_types>

// <ms_docref_add_msal>
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("AuthZPolicy", policyBuilder =>
        policyBuilder.Requirements.Add(
            new ScopeOrAppPermissionAuthorizationRequirement() {
                RequiredScopesConfigurationKey = $"AzureAd:Scopes",
                RequiredAppPermissionsConfigurationKey = $"AzureAd:AppPermissions" }
            ));
});
// </ms_docref_add_msal>

IdentityModelEventSource.ShowPII = true;

// <ms_docref_enable_authz_capabilities>
WebApplication app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// </ms_docref_enable_authz_capabilities>

var weatherSummaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
weatherSummaries.Append("BLARG");

// <ms_docref_protect_endpoint>
app.MapGet("/weatherforecast", [Authorize(Policy = "AuthZPolicy")] (HttpContext context) =>
{
    ClaimsPrincipal apiUser = context.User;
    string? userObjectID = apiUser.FindFirst(ClaimConstants.ObjectId)?.Value;
    
    if (!String.IsNullOrEmpty(userObjectID) && userObjectID == "3d6908ca-a97c-453b-ab49-3d1a407b6af6")
    {
        Console.WriteLine("==-==-==-==-==-==-==-==-== CASEY CHECKED THE WEATHER ==-==-==-==-==-==-==-==-==");
    }
    
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           weatherSummaries[Random.Shared.Next(weatherSummaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
// </ms_docref_protect_endpoint>

// TESTING ONLY
// Call with:
// curl -X POST https://localhost:5001/addweathersummary?summary=BLARG -ki -H "Content-Type: application/x-www-form-urlencoded"
app.MapPost("/addweathersummary", (string summary) =>
{
    return weatherSummaries.Append(summary);
})
.WithName("AddWeatherSummary");

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}