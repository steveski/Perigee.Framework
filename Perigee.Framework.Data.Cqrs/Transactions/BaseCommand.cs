namespace Perigee.Cqrs.Base.Transactions
{
    using System.Security.Principal;

    public abstract class BaseCommand : IDefineCommand
    {
        public IPrincipal Principal { get; set; }
    }
}