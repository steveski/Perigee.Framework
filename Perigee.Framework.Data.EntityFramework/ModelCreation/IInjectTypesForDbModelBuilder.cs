namespace Perigee.EntityFramework.ModelCreation
{
    using System;
    using System.Collections.Generic;

    public interface IInjectTypesForDbModelBuilder
    {
        List<Type> Types { get; }
    }
}