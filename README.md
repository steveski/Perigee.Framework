![CI](https://github.com/steveski/Perigee.Framework/workflows/CI/badge.svg)

# Perigee.Framework
Build .NET application using CQRS easily.
This is an implementation of Steven van Deurson's blogs arcticles on his [Command](https://blogs.cuttingedge.it/steven/posts/2011/meanwhile-on-the-command-side-of-my-architecture/) / [Query](https://blogs.cuttingedge.it/steven/posts/2011/meanwhile-on-the-command-side-of-my-architecture/) architecture. Some extensions of Steven's work and core concepts from the project [Tripod](https://github.com/danludwig/tripod) have been reworked into a more generic base framework that can be used in any project type. The project compiles as a suite of .NET Standard libraries allowing use in a variety of .NET language versions.

Examples of usage can be found in the [wiki](https://github.com/steveski/Perigee.Framework/wiki).

### Installation

For a minimal setup providing a CQRS pipeline to start creating commands and queries:

    Install-Package Perigee.Framework.Services
    
Perigee.Framework.Services and Perigee.Framework.Base will be added to your project.
If you wish to add database operations, you can add the Perigee.Framework.EntityFramework integration package:

    Install-Package Perigee.Framework.EntityFramework

If you have a need for another ORM then further projects can be added which implement the CQRS base library interfaces allowing for a clean drop in replacement.

