namespace ExampleRestApi.Contract
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Customer")]
    public class CustomerDto
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string firstName { get; set; }

        [DataMember]
        public string lastName { get; set; }

        [DataMember]
        public string emailAddress { get; set; }

        [DataMember]
        public int? addressId { get; set; }
    }
}
