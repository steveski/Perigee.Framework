namespace Perigee.Framework.Base.Services
{
    using System;

    public interface IDateTimeService
    {
        public DateTime Now { get; }
        public DateTime Today { get; }
        public DateTime UtcNow { get; }

    }
}
