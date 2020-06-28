namespace Perigee.Framework.Cqrs.Entities
{
    public interface ITimestampEnabled
    {
        byte[] Version { get; set; }
    }
}