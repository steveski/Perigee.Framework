namespace Perigee.Framework.EntityFramework
{
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Entities;
    using Perigee.Framework.Base.Services;

    public class DefaultAuditedEntityUpdater : IAuditedEntityUpdater
    {
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public DefaultAuditedEntityUpdater(IUserService userService, IDateTimeService dateTimeService)
        {
            Ensure.Any.IsNotNull(userService, nameof(userService));
            Ensure.Any.IsNotNull(dateTimeService, nameof(dateTimeService));

            _userService = userService;
            _dateTimeService = dateTimeService;

        }

        public void UpdateAuditFields(IEnumerable<object> addedEntities, IEnumerable<object> updatedEntities)
        {
            var user = _userService?.ClaimsPrincipal?.Identity?.Name ?? "Unknown";
            var now = _dateTimeService.Now;

            foreach (var added in addedEntities.OfType<IAuditedEntity>())
            {
                added.CreatedBy = user;
                added.CreatedOn = now;

                added.UpdatedBy = user;
                added.UpdatedOn = now;

            }

            foreach (var added in updatedEntities.OfType<IAuditedEntity>())
            {
                added.UpdatedBy = user;
                added.UpdatedOn = now;

            }

        }

    }
}
