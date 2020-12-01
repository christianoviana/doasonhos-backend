namespace PucMinas.Services.Charity.Domain.ValueObject
{
    public class Address
    {
        public Address()
        {

        }

        public Address(string city, string state, string country, string cep, string addressName, string number)
        {
            City = city;
            State = state;
            Country = country;
            CEP = cep;
            AddressName = addressName;
            Number = number;
        }

        public Address(string city, string state, string country, string cEP, string addressName, string district, string number, string complement)
        {
            City = city;
            State = state;
            Country = country;
            CEP = cEP;
            AddressName = addressName;
            District = district;
            Number = number;
            Complement = complement;
        }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }         
        public string CEP { get; set; }
        public string AddressName { get; set; }
        public string District { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
    }
}