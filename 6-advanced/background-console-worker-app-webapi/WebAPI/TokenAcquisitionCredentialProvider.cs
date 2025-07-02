using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPI
{
    /// <summary>
    /// Class used to add the bearer token header with the access token obtained for the signed user
    /// </summary>
    internal class TokenAcquisitionCredentialProvider : IAuthenticationProvider
    {
        public TokenAcquisitionCredentialProvider(ITokenAcquisition tokenAcquisition, IEnumerable<string> initialScopes)
        {
            _tokenAcquisition = tokenAcquisition;
            _initialScopes = initialScopes;
        }

        ITokenAcquisition _tokenAcquisition;
        IEnumerable<string> _initialScopes;

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_initialScopes,
                tokenAcquisitionOptions: new TokenAcquisitionOptions()
                {
                    LongRunningWebApiSessionKey = TokenAcquisitionOptions.LongRunningWebApiSessionKeyAuto
                });

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
        }
    }
}
