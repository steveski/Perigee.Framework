namespace Perigee.EntityFramework.ModelCreation
{
    using Microsoft.EntityFrameworkCore;

    public interface ICreateDbModel
    {
        void Create(ModelBuilder modelBuilder);
    }
}