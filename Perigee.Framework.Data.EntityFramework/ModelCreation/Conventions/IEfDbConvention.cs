namespace Perigee.Framework.Data.EntityFramework.ModelCreation.Conventions
{
    using Microsoft.EntityFrameworkCore;

    public interface IEfDbConvention
    {
        void SetConvention(ModelBuilder modelBuilder);
    }
}