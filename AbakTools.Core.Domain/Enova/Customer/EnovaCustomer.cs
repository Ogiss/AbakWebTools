namespace AbakTools.Core.Domain.Enova.Customer
{
    public class EnovaCustomer : GuidedEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Nip { get; set; }
        public string Email { get; set; }
        public int PaymentDeadlineInDays { get; set; }
        //public Address.Address MainAddress { get; set; }

    }
}
