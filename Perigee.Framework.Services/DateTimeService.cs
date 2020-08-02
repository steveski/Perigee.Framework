namespace Perigee.Framework.Services
{
    using System;
    using Perigee.Framework.Base.Services;

    /// <summary>
    /// Service to provide the current DateTime. Overrides possible
    /// </summary>
    public class DateTimeService : IDateTimeService
    {
        private readonly IDateTimeConfig _config;

        public DateTimeService(IDateTimeConfig config)
        {
            _config = config ?? new DateTimeConfig();

        }

        public DateTime Now
        {
            get
            {
                if (_config.OverrideDateTime.HasValue)
                    return _config.OverrideDateTime.Value;

                if (_config.UseLocalTime)
                    return DateTime.Now;

                return DateTime.UtcNow;
            }
        }
    
        public DateTime Today {
            get
            {
                if (_config.OverrideDateTime.HasValue)
                    return _config.OverrideDateTime.Value.Date;

                if (_config.UseLocalTime)
                    return DateTime.Now.Date;

                return DateTime.UtcNow.Date; 
            }
        }

    }
}
