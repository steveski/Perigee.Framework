namespace Perigee.Framework.Base.Services
{
    using System.Security.Claims;

    public interface IPrincipalProvider
    {
        ClaimsPrincipal ClaimsPrincipal { get; }

    }
}
