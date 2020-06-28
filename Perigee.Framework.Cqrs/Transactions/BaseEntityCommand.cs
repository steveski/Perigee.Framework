namespace Perigee.Framework.Cqrs.Transactions
{
    public abstract class BaseEntityCommand
    {
        protected BaseEntityCommand()
        {
            Commit = true;
        }

        public bool Commit { get; set; } // TODO: See if this can remain internal
    }
}