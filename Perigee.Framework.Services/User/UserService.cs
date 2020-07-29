namespace Perigee.Framework.Services.User
{
    using System.Runtime.InteropServices;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Perigee.Framework.Base.Services;

    public class UserService : IUserService
    {
        private readonly IPrincipalProvider _principalProvider;

        public UserService(IPrincipalProvider principalProvider)
        {
            _principalProvider = principalProvider;
        }

        public ClaimsPrincipal ClaimsPrincipal
        {
            get
            {
                var task = Task.Run(() => _principalProvider.GetClaimsPrincipal());
                var claimsPrincipal = task.GetAwaiter().GetResult();
                return claimsPrincipal;
            }
        }

        public IIdentity ClaimsIdentity
        {
            get
            {
                var claimsIdentity = ClaimsPrincipal.Identity as ClaimsIdentity;
                if (ClaimsPrincipal.Identities == null)
                    return claimsIdentity;

                foreach (var identity in ClaimsPrincipal.Identities)
                {
                    if (identity != null && identity.IsAuthenticated && string.IsNullOrWhiteSpace(identity.Name) == false)
                    {
                        claimsIdentity = identity;
                        break;
                    }
                }

                return claimsIdentity;
            }
        }


    }
}
