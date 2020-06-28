namespace Perigee.Framework.Base.Transactions
{
    using System.Security.Principal;

    public interface IDefineSecuredCommand : IDefineCommand
    {
        IPrincipal Principal { get; set; }
    }
}