namespace Perigee.Framework.Services
{
    using System;

    public class DateTimeConfig : IDateTimeConfig
    {
        public bool UseLocalTime { get; set; } = false;

        public DateTime? OverrideDateTime { get; set; }

    }
}