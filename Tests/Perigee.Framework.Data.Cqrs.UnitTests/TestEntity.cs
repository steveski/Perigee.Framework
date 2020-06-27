namespace Perigee.Framework.Data.Cqrs.UnitTests
{
    using Entities;

    public class TestEntity : EntityWithId<int>
    {
        public string Name { get; set; }

    }
}
