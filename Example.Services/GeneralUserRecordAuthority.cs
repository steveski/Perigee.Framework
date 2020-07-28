namespace Example.Services
{
    using System;
    using System.Linq.Expressions;
    using Example.Entities;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Entities;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Services.User;

    public class GeneralUserRecordAuthority : IRecordAuthority
    {
        private readonly IUserService _userService;

        public GeneralUserRecordAuthority(IUserService userService)
        {
            _userService = userService;

        }

        public Expression<Func<TEntity, bool>> Clause<TEntity>() where TEntity : IEntity
        {
            if (typeof(TEntity) == typeof(Customer))
            {
                var name = _userService.ClaimsIdentity.Name;
                return x => ((Customer)(object)x).ManagedBy == name;
            }

            // For each table / entity provide the expressions that match the filtering required for the specific entity


            return x => true;
        }
 
    }
}
