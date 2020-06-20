namespace Perigee.EntityFramework.Initialisation
{
    using Microsoft.EntityFrameworkCore;

    public class BrownfieldDbInitialiser //: IDatabaseInitializer<DbContext>
    {
        public void InitializeDatabase(DbContext db)
        {
            // assume db already exists and should not be fooled with
        }
    }
}