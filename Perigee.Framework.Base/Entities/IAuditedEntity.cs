namespace Perigee.Framework.Base.Entities
{
    using System;

    public interface IAuditedEntity
    {
        DateTime? CreatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        string UpdatedBy { get; set; }
    }

    //public abstract class AuditedEntity : IAuditedEntity
    //{
    //    //auditing properties
    //    public virtual DateTime? CreatedOn { get; set; }
    //    public virtual string CreatedBy { get; set; }
    //    public virtual DateTime? UpdatedOn { get; set; }
    //    public virtual string UpdatedBy { get; set; }

    //}
}