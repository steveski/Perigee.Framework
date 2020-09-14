using Perigee.Framework.Base.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Example.Portal.Entities
{
    public class Customer : Entity<Guid>, IAuditedEntity, ITimestampEnabled, ISoftDelete
    {
        [MaxLength(50)]
        public string customerName { get; set; }

        #region IAuditedEntity Implementation
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        #endregion

        #region ITimestampEnabled Implementation
        public byte[] Version { get; set; }
        #endregion

        #region ISoftDelete Implementation
        public bool IsDeleted { get; set; }
        #endregion
    }
}
