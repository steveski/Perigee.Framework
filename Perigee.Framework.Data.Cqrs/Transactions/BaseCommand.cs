namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System.Security.Principal;

    public abstract class BaseCommand : IDefineCommand
    {
        public IPrincipal Principal { get; set; }
    }
}