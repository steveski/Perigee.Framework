namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System.Threading.Tasks;

    public interface IPopulateCommand<in TCommand> where TCommand : IDefineCommand
    {
        Task Populate(TCommand command);
    }
}