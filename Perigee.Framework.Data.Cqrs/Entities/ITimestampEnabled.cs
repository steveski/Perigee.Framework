namespace Perigee.Cqrs.Base.Entities
{
    public interface ITimestampEnabled
    {
        byte[] Version { get; set; }
    }
}