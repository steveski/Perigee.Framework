namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System.Security.Principal;
    using Helpers.Shared;

    public interface IDefineSecuredCommand : IDefineCommand
    {
        IPrincipal Principal { [UsedImplicitly] get; set; }
    }
}