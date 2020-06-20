﻿namespace Perigee.Framework.Data.EntityFramework.ModelCreation
{
    using System;
    using System.Collections.Generic;

    public interface IInjectTypesForDbModelBuilder
    {
        List<Type> Types { get; }
    }
}