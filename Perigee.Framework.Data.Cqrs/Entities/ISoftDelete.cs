namespace Perigee.Framework.Data.Cqrs.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}