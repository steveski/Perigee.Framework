namespace Perigee.Framework.Base.Transactions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class BasePopulateCommand<TCommand> : IPopulateCommand<TCommand> where TCommand : IDefineCommand
    {
        private bool _isPopulated;

        protected BasePopulateCommand()
        {
            PopulateAction = cmd => { };
        }


        protected Action<TCommand> PopulateAction { get; set; }

        public virtual Task Populate(TCommand command, CancellationToken cancellationToken)
        {
            PopulateAction?.Invoke(command);

            return Task.CompletedTask;
        }

        public async Task<bool> HandlePopulate(TCommand command, CancellationToken cancellationToken)
        {
            if (!_isPopulated)
                await Populate(command, cancellationToken).ConfigureAwait(false);

            _isPopulated = true;
            return true;
        }
    }
}