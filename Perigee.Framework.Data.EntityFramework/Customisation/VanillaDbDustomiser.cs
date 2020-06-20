namespace Perigee.Framework.Data.EntityFramework.Customisation
{
    using Helpers.Shared;
    using Microsoft.EntityFrameworkCore;

    [UsedImplicitly]
    public class VanillaDbCustomiser : ICustomiseDb
    {
        public void Customise(DbContext dbContext)
        {
            // do not customise
        }
    }
}