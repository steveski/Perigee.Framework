namespace Perigee.Framework.Base.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}