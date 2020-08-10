namespace Perigee.Framework.EntityFramework.IntegrationTests.Domain
{
    using System.Reflection.Metadata.Ecma335;
    using System.Threading;
    using System.Threading.Tasks;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting;

    public class CreatePersonCommand : BaseCreateEntityCommand<Person>
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; }

    }

    public class HandleCreatePersonCommand : IHandleCommand<CreatePersonCommand>
    {
        private readonly IWriteEntities _db;

        public HandleCreatePersonCommand(IWriteEntities db)
        {
            _db = db;

        }

        public Task Handle(CreatePersonCommand command, CancellationToken cancellationToken)
        {
            var newPerson = new Person
            {
                 Name = command.Name,
                 Age = command.Age,
                 Description = command.Description

            };

            _db.Create(newPerson);
            command.CreatedEntity = newPerson;

            return Task.CompletedTask;
        }

    }

}
