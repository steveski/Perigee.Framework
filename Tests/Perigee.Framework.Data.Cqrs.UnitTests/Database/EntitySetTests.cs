namespace Perigee.Framework.Data.Cqrs.UnitTests.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Cqrs.Database;
    using FluentAssertions;
    using NSubstitute;
    using Xunit;

    public class EntitySetTests
    {
        [Fact]
        public void ThrowsExceptionWhenCreatedWithNullQueryable()
        {
            var readEntities = Substitute.For<IReadEntities>();
            
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new EntitySet<TestEntity>(null, readEntities);

            action.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void ThrowsExceptionWhenCreatedWithNullEntities()
        {
            var queryable = new List<TestEntity>().AsQueryable();

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new EntitySet<TestEntity>(queryable, null);

            action.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void GetEnumeratorDoesNotThrowNullReferenceException()
        {
            var queryable = new List<TestEntity>().AsQueryable();
            var readEntities = Substitute.For<IReadEntities>();

            var sut = new EntitySet<TestEntity>(queryable, readEntities);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action action = () => sut.GetEnumerator();

            action.Should().NotThrow<NullReferenceException>();

        }






        [Fact]
        public void ExpreessionPropertyDoesNotThrowNullReferenceException()
        {
            var queryable = new List<TestEntity>().AsQueryable();
            var readEntities = Substitute.For<IReadEntities>();

            var sut = new EntitySet<TestEntity>(queryable, readEntities);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Func<Expression> exp = () => sut.Expression;

            exp.Should().NotThrow<NullReferenceException>();

        }

        [Fact]
        public void ElementTypeDoesNotThrowNullReferenceException()
        {
            var queryable = new List<TestEntity>().AsQueryable();
            var readEntities = Substitute.For<IReadEntities>();

            var sut = new EntitySet<TestEntity>(queryable, readEntities);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Func<Type> type = () => sut.ElementType;

            type.Should().NotThrow<NullReferenceException>();
        }

        [Fact]
        public void ProviderDoesNotThrowNullReferenceException()
        {
            var queryable = new List<TestEntity>().AsQueryable();
            var readEntities = Substitute.For<IReadEntities>();

            var sut = new EntitySet<TestEntity>(queryable, readEntities);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Func<IQueryProvider> provider = () => sut.Provider;

            provider.Should().NotThrow<NullReferenceException>();

        }

        [Fact]
        public void ElementTypeReturnsCorrectTEntity()
        {
            var queryable = new List<TestEntity>().AsQueryable();
            var readEntities = Substitute.For<IReadEntities>();

            var sut = new EntitySet<TestEntity>(queryable, readEntities);
            var type = sut.ElementType;

            type.Should().Be<TestEntity>();

        }


    }
}
