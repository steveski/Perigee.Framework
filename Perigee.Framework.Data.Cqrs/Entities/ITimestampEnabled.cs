namespace Perigee.Framework.Data.Cqrs.Entities
{
    public interface ITimestampEnabled
    {
        byte[] Version { get; set; }
    }
}