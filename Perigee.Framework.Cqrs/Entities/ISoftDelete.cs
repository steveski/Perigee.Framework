namespace Perigee.Framework.Cqrs.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}