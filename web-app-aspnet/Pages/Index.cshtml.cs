using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;

namespace sign_in_webapp.Pages;

[AuthorizeForScopes(ScopeKeySection = "DownstreamApis:MicrosoftGraph:Scopes")]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly IDownstreamApi  _downstreamWebApi;

    public IndexModel(ILogger<IndexModel> logger,
                        IDownstreamApi  downstreamWebApi)
    {
        _logger = logger;
        _downstreamWebApi = downstreamWebApi;
    }

    public async Task OnGet()
    {
        using var response = await _downstreamWebApi.CallApiForUserAsync("MicrosoftGraph").ConfigureAwait(false);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var apiResult = await response.Content.ReadFromJsonAsync<JsonDocument>().ConfigureAwait(false);
            ViewData["ApiResult"] = JsonSerializer.Serialize(apiResult, new JsonSerializerOptions { WriteIndented = true });
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}: {error}");
        }
    }
}
