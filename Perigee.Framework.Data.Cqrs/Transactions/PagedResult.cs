namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System.Collections.ObjectModel;

    public sealed class PagedResult<T>
    {
        public readonly int PageCount;

        public readonly PagingInformation Paging;
        public readonly ReadOnlyCollection<T> Rows; // Page

        // TODO: renaming these fields to align with what JS expects is a bit of a hack. they were called RowCount and Page respectively

        public readonly int Total; // RowCount

        public PagedResult(PagingInformation paging, int pageCount, int itemCount, ReadOnlyCollection<T> page)
        {
            Paging = paging;
            PageCount = pageCount;
            Total = itemCount;
            Rows = page;
        }
    }
}