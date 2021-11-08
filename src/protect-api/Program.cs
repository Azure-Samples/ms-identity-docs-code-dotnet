// <ms_docref_import_types>
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
// </ms_docref_import_types>

var builder = WebApplication.CreateBuilder(args);

// <ms_docref_add_msal>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
// </ms_docref_add_msal>
// <ms_docref_add_default_authz_policies>
builder.Services.AddAuthorization();
// </ms_docref_add_default_authz_policies>

var app = builder.Build();

app.UseHttpsRedirection();

// <ms_docref_enable_authn_capabilities>
app.UseAuthentication();
// </ms_docref_enable_authn_capabilities>
// <ms_docref_enable_authz_capabilities>
app.UseAuthorization();
// </ms_docref_enable_authz_capabilities>

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// <ms_docref_protect_endpoint>
app.MapGet("/weatherforecast", [Authorize] () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
// </ms_docref_protect_endpoint>

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
