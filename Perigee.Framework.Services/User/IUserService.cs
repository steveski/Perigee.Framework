namespace Perigee.Framework.Services.User
{
    using System.Security.Claims;
    using System.Security.Principal;

    public interface IUserService
    {
        ClaimsPrincipal ClaimsPrincipal { get; set; }

        IIdentity ClaimsIdentity { get; }


    }
}