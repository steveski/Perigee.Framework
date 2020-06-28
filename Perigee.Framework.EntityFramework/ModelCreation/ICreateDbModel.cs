namespace Perigee.Framework.EntityFramework.ModelCreation
{
    using Microsoft.EntityFrameworkCore;

    public interface ICreateDbModel
    {
        void Create(ModelBuilder modelBuilder);
    }
}