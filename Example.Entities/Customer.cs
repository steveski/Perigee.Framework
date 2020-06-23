namespace Example.Entities
{
    using System;
    using System.Collections.Generic;
    using Perigee.Framework.Data.Cqrs.Entities;

    public class Customer : EntityWithId<int>, IAuditedEntity, ITimestampEnabled
    {
        public Customer()
        {
            
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string EmailAddress { get; set; }



        #region IAuditedEntity Implementation

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        #endregion

        #region IAuditedEntity Implementation
    
        public byte[] Version { get; set; }

        #endregion

    }
}