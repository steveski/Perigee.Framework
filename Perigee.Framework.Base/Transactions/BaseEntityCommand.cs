namespace Perigee.Framework.Base.Transactions
{
    public abstract class BaseEntityCommand : BaseCommand
    {
        public bool Commit { get; set; } = true;
    }
}