namespace Perigee.Framework.Cqrs.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Entities;

    public abstract class BaseQueryEnumerable<T>
    {
        public IDictionary<Expression<Func<T, object>>, OrderByDirection> OrderBy { get; set; }
    }
}