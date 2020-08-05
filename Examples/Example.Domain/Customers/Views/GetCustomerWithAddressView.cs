namespace Example.Domain.Customers.Views
{
    public class GetCustomerWithAddressView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
