namespace Perigee.Cqrs.Base.Transactions
{
    using System;

    public class PagingInformation
    {
        public readonly int PageIndex;
        public readonly int PageSize;
        public int ItemCount;

        public int PageCount;

        public PagingInformation(int pageIndex, int pageSize)
        {
            if (pageIndex < 0) throw new ArgumentOutOfRangeException(nameof(pageIndex));
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}