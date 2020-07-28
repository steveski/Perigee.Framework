namespace Perigee.Framework.Services.Security
{
    using System.Security.Claims;
    using Perigee.Framework.Base.Services;

    public class PrincipalProvider : IPrincipalProvider
    {
        public PrincipalProvider(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;

        }

        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

    }
}
