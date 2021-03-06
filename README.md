![CI](https://github.com/steveski/Perigee.Framework/workflows/CI/badge.svg)

[![Nuget](https://img.shields.io/nuget/v/Perigee.Framework.Base?label=Perigee.Framework.Base)](https://www.nuget.org/packages/Perigee.Framework.Base/)
[![Nuget](https://img.shields.io/nuget/v/Perigee.Framework.Services?label=Perigee.Framework.Services)](https://www.nuget.org/packages/Perigee.Framework.Services/)
[![Nuget](https://img.shields.io/nuget/v/Perigee.Framework.EntityFramework?label=Perigee.Framework.EntityFramework)](https://www.nuget.org/packages/Perigee.Framework.EntityFramework/)
[![Nuget](https://img.shields.io/nuget/v/Perigee.Framework.Web?label=Perigee.Framework.Web)](https://www.nuget.org/packages/Perigee.Framework.Web/)

# Perigee.Framework
Build .NET application using CQRS easily.
This is an implementation of Steven van Deurson's blog arcticles on his [Command](https://blogs.cuttingedge.it/steven/posts/2011/meanwhile-on-the-command-side-of-my-architecture/) / [Query](https://blogs.cuttingedge.it/steven/posts/2011/meanwhile-on-the-command-side-of-my-architecture/) architecture. Some extensions of Steven's work and core concepts from the project [Tripod](https://github.com/danludwig/tripod) have been reworked into a more generic base framework that can be used in any project type. The project compiles as a suite of .NET Standard libraries allowing use in a variety of .NET language versions.

Examples of usage can be found in the [wiki](https://github.com/steveski/Perigee.Framework/wiki).

### Installation
For a minimal setup providing a CQRS pipeline to start creating commands and queries:

    Install-Package Perigee.Framework.Services
    
Perigee.Framework.Services and Perigee.Framework.Base will be added to your project.
If you wish to add database operations, you can add the Perigee.Framework.EntityFramework integration package:

    Install-Package Perigee.Framework.EntityFramework

If you have a need for another ORM then further projects can be added which implement the CQRS base library interfaces allowing for a clean drop in replacement.

