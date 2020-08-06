namespace Example.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Perigee.Framework.Base.Entities;

    public class Customer : Entity<int>, IAuditedEntity, ITimestampEnabled, ISoftDelete
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        //[Encrypted]
        public string EmailAddress { get; set; }

        public string ManagedBy { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public IEnumerable<CustomerEmployerMapping> CustomerEmployerMappings { get; set; }

        #region IAuditedEntity Implementation

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        #endregion

        #region ITimestampEnabled Implementation

        public byte[] Version { get; set; }

        #endregion

        public bool IsDeleted { get; set; }
    }
}