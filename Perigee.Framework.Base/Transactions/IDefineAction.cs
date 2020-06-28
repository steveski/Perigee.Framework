namespace Perigee.Framework.Base.Transactions
{
    using System.Security.Principal;

    public interface IDefineAction
    {
        IPrincipal Principal { get; set; }
    }
}