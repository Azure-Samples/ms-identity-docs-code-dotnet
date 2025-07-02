using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        /// <summary>
        /// The Web API will only accept tokens 1) for users, and 
        /// 2) having the access_as_user scope for this API
        /// </summary>
        //static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };
        private readonly GraphServiceClient _graphServiceClient;

        public ProfileController(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        /// <summary>
        /// Get the user's profile from MS Graph
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new string[] { "Profile.Read", "Profile.ReadWrite" })]
        public async Task<string> Get()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            User userProfile = await _graphServiceClient.Me
                                    .Request()
                                    .GetAsync();

            return JsonConvert.SerializeObject(new {
                country = userProfile.Country,
                city = userProfile.City,
                employeeId = userProfile.EmployeeId,
                displayName = userProfile.DisplayName,
                givenName = userProfile.GivenName,
                department = userProfile.Department,
            });
        }

    }
}
