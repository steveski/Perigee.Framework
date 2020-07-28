namespace Perigee.Framework.Base.Services
{
    using System.Security.Claims;
    using System.Security.Principal;

    public interface IUserService
    {
        ClaimsPrincipal ClaimsPrincipal { get; }

        IIdentity ClaimsIdentity { get; }


    }
}