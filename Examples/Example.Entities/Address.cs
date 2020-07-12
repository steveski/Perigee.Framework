namespace Example.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Perigee.Framework.Base.Entities;

    public class Address : Entity<int>, IAuditedEntity, ITimestampEnabled
    {

        public string Street { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public ICollection<Customer> Customers { get; set; }


        #region IAuditedEntity Implementation

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        #endregion

        #region ITimestampEnabled Implementation

        public byte[] Version { get; set; }

        #endregion

    }
}