namespace PucMinas.Services.Charity.Domain.ValueObject
{
    public class ContactNumber
    {
        public ContactNumber(string cellPhone, string telephone)
        {
            CellPhone = cellPhone;
            Telephone = telephone;
        }

        public string CellPhone { get; private set; }
        public string Telephone { get; private set; }
    }
}
