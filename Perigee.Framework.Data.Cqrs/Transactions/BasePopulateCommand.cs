﻿namespace Perigee.Cqrs.Base.Transactions
{
    using System;
    using System.Threading.Tasks;

    public abstract class BasePopulateCommand<TCommand> : IPopulateCommand<TCommand> where TCommand : IDefineCommand
    {
        private bool _isPopulated;

        protected BasePopulateCommand()
        {
            PopulateAction = cmd => { };
        }


        protected Action<TCommand> PopulateAction { get; set; }

        public virtual async Task Populate(TCommand command)
        {
            if (PopulateAction != null)
                PopulateAction(command);


            await Task.FromResult(1);
        }

        public async Task<bool> HandlePopulate(TCommand command)
        {
            if (!_isPopulated)
                await Populate(command);

            _isPopulated = true;
            return true;
        }
    }
}