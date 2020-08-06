namespace Example.Domain.Addresses.Views
{
    using System;
    using System.Linq.Expressions;
    using Example.Entities;

    public class AddressView
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public static Expression<Func<Address, AddressView>> Projector = addr => new AddressView
        {
            Id = addr.Id,
            Street = addr.Street,
            Suburb = addr.Suburb,
            PostalCode = addr.PostalCode,
            State = addr.State,
            Country = addr.Country
        };

    }
}
