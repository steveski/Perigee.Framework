namespace Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting
{
    using Perigee.Framework.Base.Entities;

    public class Person : Entity<int>, ISoftDelete
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; } = "No Description Set";
        public bool Dead { get; set; } = false;
        public bool Alive { get; set; } = true;

        public bool IsDeleted { get; set; }

    }
}
