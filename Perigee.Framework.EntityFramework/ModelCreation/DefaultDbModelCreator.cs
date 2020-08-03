namespace Perigee.Framework.EntityFramework.ModelCreation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Conventions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    public class DefaultDbModelCreator : ICreateDbModel
    {
        public void Create(ModelBuilder modelBuilder)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblies = allAssemblies.Where(assemb => NotSystemAssembly(assemb.FullName)).ToList();
            assemblies.ForEach(assemb =>
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assemb,
                    t => t.GetTypeInfo().ImplementedInterfaces.Any(i =>
                             i.IsGenericType &&
                             i.Name.Equals(typeof(IEntityTypeConfiguration<>).Name,
                                 StringComparison.InvariantCultureIgnoreCase)
                         ) &&
                         t.IsClass &&
                         !t.IsAbstract &&
                         !t.IsNested);
            });


            var assembly = Assembly.GetAssembly(GetType());

            // Set "Conventions" using an alternate technique until Conventions are supported in EF Core
            var dbConventions = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().ImplementedInterfaces
                    .Any(i => i.Name.Equals(nameof(IEfDbConvention), StringComparison.InvariantCultureIgnoreCase)
                    )).ToList();
            dbConventions.ForEach(t =>
            {
                var instance = (IEfDbConvention) assembly.CreateInstance(t.FullName);
                instance.SetConvention(modelBuilder);
            });


            // This will singularize all table names
            ////foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            ////{
            ////    entityType.Relational().TableName = entityType.DisplayName();
            ////}

  
        }

        private bool NotSystemAssembly(string assembFullName)
        {
            bool ret = !(assembFullName.StartsWith("System.", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("Microsoft.", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("netstandard", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("Remoting.", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("NewtonSoft", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("FluentValidation", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("SimpleInjector", StringComparison.InvariantCultureIgnoreCase) ||
                         assembFullName.StartsWith("Anonymous", StringComparison.InvariantCultureIgnoreCase));

            return ret;
        }
    }
}