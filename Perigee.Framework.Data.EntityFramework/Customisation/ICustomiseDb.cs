namespace Perigee.EntityFramework.Customisation
{
    using Microsoft.EntityFrameworkCore;

    public interface ICustomiseDb
    {
        void Customise(DbContext dbContext);
    }
}