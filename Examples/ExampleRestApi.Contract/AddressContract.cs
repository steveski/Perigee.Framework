namespace ExampleRestApi.Contract
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Address")]
    public class AddressDto
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string street { get; set; }

        [DataMember]
        public string suburb { get; set; }

        [DataMember]
        public string postalCode { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string country { get; set; }
    }
}
