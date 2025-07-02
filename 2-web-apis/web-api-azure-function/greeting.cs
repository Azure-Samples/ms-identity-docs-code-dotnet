/*
This is an Azure Function that responds at GET /api/greeting.

Using the built-in authentication and authorization capabilities (sometimes
referred to as "Easy Auth") of Azure Functions, offloads part of the authentication
and authorization process by ensuring that every request to this Azure Function has
an access token. That access token has had its signature, issuer (iss), expiry
dates (exp, nbf), and audience (aud) validated. This means all that is left to
perform is any per-function authorization related to your application.
*/

using System;
using System.IO;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace Api
{
    public static class Greeting
    {
        /*
        Azure Functions HTTP Triggers perform automatic input binding of the
        Easy Auth-validated JWT token data into the ClaimsPincipal.
        Using the claims principal allows additional access token validation specific to this function.
        */
        [FunctionName("greeting")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            ClaimsPrincipal principal)
        {
            // This API endpoint requires the "Greeting.Read" scope to be present, if it is
            // not, then reject the request with a 403.
            if (!principal.Claims.Any(
                  c => c.Type == "http://schemas.microsoft.com/identity/claims/scope"
                  && c.Value.Split(' ').Contains("Greeting.Read")))
            {
                return new ObjectResult("Forbidden") { StatusCode = 403};
            }

            // Authentication is complete, process request.
            return new OkObjectResult("Hello, world. You were able to access this because you provided a valid access token with the Greeting.Read scope as a claim.");
        }
    }
}
