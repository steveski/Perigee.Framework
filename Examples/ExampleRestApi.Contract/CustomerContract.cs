namespace ExampleRestApi.Contract
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Contract")]
    public class CustomerContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }


    }
}
