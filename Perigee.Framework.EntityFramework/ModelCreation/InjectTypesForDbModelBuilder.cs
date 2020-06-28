namespace Perigee.Framework.EntityFramework.ModelCreation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;

    public class InjectTypesForDbModelBuilder : IInjectTypesForDbModelBuilder
    {
        public List<Type> Types
        {
            get
            {
                var assembly = Assembly.GetAssembly(GetType());
                return assembly.GetTypes()
                    .Where(t => !t.IsAbstract &&
                                typeof(IEntityTypeConfiguration<>)
                                    .ContainsGenericParameters) // TODO: Find out if this is the way to go - IsGenericallyAssignableFrom(t))
                    .ToList();
            }
        }
    }
}