namespace Example.Entities
{
    using System;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Entities;

    public class Employment : Entity<int>, IHistoryEnabled
    {
        public string Position { get; set; }
        public string Responsibilities { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ActingRole { get; set; }



    }
}
