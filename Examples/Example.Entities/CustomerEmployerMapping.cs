using Perigee.Framework.Base.Entities;

namespace Example.Entities
{
    public class CustomerEmployerMapping : Entity<int>
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int EmployerId { get; set; }
        public Employer Employer { get; set; }
    }
}
