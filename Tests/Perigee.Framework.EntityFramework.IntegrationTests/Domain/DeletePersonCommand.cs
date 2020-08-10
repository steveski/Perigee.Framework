namespace Perigee.Framework.EntityFramework.IntegrationTests.Domain
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting;

    public class DeletePersonCommand : BaseCreateEntityCommand<Person>
    {
        public string PersonName { get; set; }

    }

    public class HandlerDeletePersonCommand : IHandleCommand<DeletePersonCommand>
    {
        private readonly IWriteEntities _db;

        public HandlerDeletePersonCommand(IWriteEntities db)
        {
            _db = db;

        }

        public async Task Handle(DeletePersonCommand command, CancellationToken cancellationToken)
        {
            var person = await _db.Get<Person>().SingleOrDefaultAsync(p => p.Name == command.PersonName, cancellationToken).ConfigureAwait(false);
            if (person == null)
                return;

            _db.Delete(person);

        }

    }

}