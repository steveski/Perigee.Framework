namespace Perigee.Framework.EntityFramework.IntegrationTests.Domain
{
    using System.Reflection.Metadata.Ecma335;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting;

    public class CreatePersonCommand : BaseCreateEntityCommand<Person>
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; } = "No Description Set";
        public bool Dead { get; set; } = false;
        public bool Alive { get; set; } = true;
    }

    public class HandleCreatePersonCommand : IHandleCommand<CreatePersonCommand>
    {
        private readonly IWriteEntities _db;
        private readonly IMapper _mapper;

        public HandleCreatePersonCommand(IWriteEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task Handle(CreatePersonCommand command, CancellationToken cancellationToken)
        {
            var newPerson = _mapper.Map<CreatePersonCommand, Person>(command);
            /*
            var newPerson = new Person
            {
                Name = command.Name,
                Age = command.Age,
                Description = command.Description,
                Alive = command.Alive,
                Dead = command.Dead
            };
*/

            _db.Create(newPerson);
            command.CreatedEntity = newPerson;

            return Task.CompletedTask;
        }

    }

}
