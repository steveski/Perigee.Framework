namespace Perigee.Framework.Services.Security
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Perigee.Framework.Base.Services;

    public class PrincipalProvider : IPrincipalProvider
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        public PrincipalProvider(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }

        public Task<ClaimsPrincipal> GetClaimsPrincipal() => Task.FromResult(_claimsPrincipal);

    }
}
