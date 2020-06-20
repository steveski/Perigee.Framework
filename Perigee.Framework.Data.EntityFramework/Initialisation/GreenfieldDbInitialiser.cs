namespace Perigee.Framework.Data.EntityFramework.Initialisation
{
    using Customisation;
    using Helpers.Shared;

    [UsedImplicitly]
    public class GreenfieldDbInitialiser //: DropCreateDatabaseIfModelChanges<DbContext>
    {
        private readonly ICustomiseDb _customizer;

        public GreenfieldDbInitialiser(ICustomiseDb customizer)
        {
            _customizer = customizer;
        }

        //    protected override void Seed(DbContext db)
        //    {
        //        if (_customizer != null) _customizer.Customise(db);
        //    }
    }
}