﻿namespace Perigee.Framework.Base.Transactions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPopulateCommand<in TCommand> where TCommand : IDefineCommand
    {
        Task Populate(TCommand command, CancellationToken cancellationToken);
    }
}