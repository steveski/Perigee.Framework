namespace Example.Entities
{
    using System;
    using Perigee.Framework.Base.Entities;

    public class Customer : Entity<int>, IAuditedEntity, ITimestampEnabled
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string ManagedBy { get; set; }


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