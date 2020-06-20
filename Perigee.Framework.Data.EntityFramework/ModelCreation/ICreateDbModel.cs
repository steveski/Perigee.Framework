namespace Perigee.Framework.Data.EntityFramework.ModelCreation
{
    using Microsoft.EntityFrameworkCore;

    public interface ICreateDbModel
    {
        void Create(ModelBuilder modelBuilder);
    }
}