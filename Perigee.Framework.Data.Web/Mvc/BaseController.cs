namespace Perigee.Framework.Data.Web.Mvc
{
    using Cqrs.Transactions;
    using Microsoft.AspNetCore.Mvc;

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