namespace Perigee.Cqrs.Base.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Entities;

    /// <summary>
    ///     Used to return a paginated list of views. To return all views without pagination, set limit=0. Will still return as
    ///     a single page of information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseQueryPaginated<T> : BaseQuery<PagedResult<T>>
    {
        public BaseQueryPaginated()
        {
            OrderBy = new Dictionary<Expression<Func<T, object>>, OrderByDirection>();
        }

        public IDictionary<Expression<Func<T, object>>, OrderByDirection> OrderBy { get; set; }

        public string sort { get; set; } //OrderByProperty
        public string order { get; set; } //OrderByDirection


        public int offset { get; set; } // number of records to offset, e.g. can calculate page size
        public int limit { get; set; } // number of records on a page


        //public PagingInformation PagingInformation { get; set; }
        public PagingInformation PagingInformation =>
            limit == 0
                ? null // no paging info = will not apply paging
                : new PagingInformation(offset / limit, limit);
    }
}