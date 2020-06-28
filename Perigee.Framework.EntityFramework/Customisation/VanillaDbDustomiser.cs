namespace Perigee.Framework.EntityFramework.Customisation
{
    using Microsoft.EntityFrameworkCore;

    public class VanillaDbCustomiser : ICustomiseDb
    {
        public void Customise(DbContext dbContext)
        {
            // do not customise
        }
    }
}