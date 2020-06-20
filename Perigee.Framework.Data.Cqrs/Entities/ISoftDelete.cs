namespace Perigee.Cqrs.Base.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}