namespace Perigee.Framework.Services
{
    using System;

    public interface IDateTimeConfig
    {
        bool UseLocalTime { get; }
        
        DateTime? OverrideDateTime { get; }

    }
}
