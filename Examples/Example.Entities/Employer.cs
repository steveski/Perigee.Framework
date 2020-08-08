using Perigee.Framework.Base.Entities;
using System.Collections;
using System.Collections.Generic;

namespace Example.Entities
{
    public class Employer : Entity<int>
    {
        public string Name { get; set; }

        public IEnumerable<CustomerEmployerMapping> CustomerEmployerMappings { get; set; }
    }
}
