namespace Example.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Perigee.Framework.Base.Transactions;

    public class CommandProcessorQueue : ICommandProcessorQueue
    {
        private readonly IProcessCommands _processCommands;

        private readonly BlockingCollection<IDefineCommand> _commandQueue = new BlockingCollection<IDefineCommand>();


        public CommandProcessorQueue(IProcessCommands processCommands)
        {
            _processCommands = processCommands;

        }

        public async Task StartProcessing(CancellationToken cancellationToken)
        {
            var processingTask = Task.Run(async () =>
            {
                try
                {
                    // Consume consume the BlockingCollection
                    while (true)
                    {
                        var currentCommand = _commandQueue.Take();
                        await _processCommands.Execute(currentCommand, cancellationToken).ConfigureAwait(false);

                    }

                }
                catch (InvalidOperationException)
                {
                    // An InvalidOperationException means that Take() was called on a completed collection
                    //Console.WriteLine("That's All!");
                }
            }, cancellationToken);

            await processingTask.ConfigureAwait(false);
        }


        public void EnqueueCommand(IDefineCommand command, CancellationToken cancellationToken)
        {
            _commandQueue.Add(command, cancellationToken);

        }

        public void FinaliseQueue()
        {
            _commandQueue.CompleteAdding();

        }

    }

    public interface ICommandProcessorQueue
    {
        Task StartProcessing(CancellationToken cancellationToken);
        void EnqueueCommand(IDefineCommand command, CancellationToken cancellationToken);
        void FinaliseQueue();

    }
}
