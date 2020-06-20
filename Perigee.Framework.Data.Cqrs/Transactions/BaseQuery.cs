namespace Perigee.Cqrs.Base.Transactions
{
    using System.Security.Principal;

    /// <summary>
    ///     Used to return a single type, which isn't a DB Context Entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseQuery<T> : IDefineQuery<T>
    {
        public IPrincipal Principal { get; set; }
    }
}