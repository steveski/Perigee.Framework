namespace Perigee.Framework.Base.Transactions
{
    public abstract class BaseEntityCommand : BaseCommand
    {
        protected BaseEntityCommand()
        {
            Commit = true;
        }

        public bool Commit { get; set; }
    }
}