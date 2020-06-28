namespace Perigee.Framework.Cqrs.UnitTests.Transactions
{
    using System;
    using Cqrs.Transactions;
    using FluentAssertions;
    using Xunit;

    public class PagingInformationTests
    {
        [Fact]
        public void ThrowsExceptionWhenCreatedWithInvalidPageIndex()
        {
            var pageIndex = -1;
            var pageSize = 1;

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new PagingInformation(pageIndex, pageSize);

            action.Should().Throw<ArgumentOutOfRangeException>();

        }

        [Fact]
        public void ThrowsExceptionWhenCreatedWithInvalidPageSize()
        {

            var pageIndex = 0;
            var pageSize = 0;

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new PagingInformation(pageIndex, pageSize);

            action.Should().Throw<ArgumentOutOfRangeException>();

        }


    }
}
