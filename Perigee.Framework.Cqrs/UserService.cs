namespace Perigee.Framework.Cqrs
{
    using System;
    using System.Security.Principal;

    public class UserService : IUserService
    {
        public IPrincipal Principal { get; set; }
        public DateTime CurrentDateTime => DateTime.Now;


    }

    public interface IUserService
    {
        IPrincipal Principal { get; set; }
        DateTime CurrentDateTime { get; }

    }

}
