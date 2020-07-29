namespace Perigee.Framework.Base.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IPrincipalProvider
    {
        Task<ClaimsPrincipal> GetClaimsPrincipal();

    }
}
