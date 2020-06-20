namespace Perigee.Web.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using Cqrs.Base.Transactions;

    public class BaseController : ControllerBase
    {
        protected readonly IProcessCommands Commands;
        protected readonly IProcessQueries Queries;

        public BaseController(IProcessQueries queries, IProcessCommands commands)
        {
            Queries = queries;
            Commands = commands;
        }
    }
}