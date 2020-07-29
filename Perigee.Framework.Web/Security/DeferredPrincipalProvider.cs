namespace Perigee.Framework.Web.Security
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.Authorization;
    using Perigee.Framework.Base.Services;

    /// <summary>
    /// Provides access to get the logged in ClaimsPrincipal via AuthenticationStateProvider.
    /// The ClaimsPrincipal is unavailable until SetAuthenticationStateProvider() has been called by some log in process within the web platform. 
    /// GetAuthenticationStateProvider() will throw an error if called first which makes using a DI "factory" style registration with lambdas impossible.
    /// </summary>
    public class DeferredPrincipalProvider : IPrincipalProvider
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public DeferredPrincipalProvider(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipal()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
            return authState.User;
        }

    }
}
