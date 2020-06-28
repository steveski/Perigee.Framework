namespace Perigee.Framework.Services
{
    using System;
    using Perigee.Framework.Base.Services;

    public class DefaultDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime Today => DateTime.Today;
        public DateTime UtcNow => DateTime.UtcNow;

    }
}
