using AutoMapper;
using Perigee.Framework.EntityFramework.IntegrationTests.Domain;
using Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting;

namespace Perigee.Framework.EntityFramework.IntegrationTests
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<CreatePersonCommand, Person>();
        }
    }
}
