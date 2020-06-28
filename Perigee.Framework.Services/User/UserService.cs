namespace Perigee.Framework.Services.User
{
    using System.Runtime.InteropServices;
    using System.Security.Claims;
    using System.Security.Principal;

    public class UserService : IUserService
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

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
