namespace AvtoSianieASP.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int ServeceId { get; set; }
        public string Massage { get; set; }
        public DateTime DateOn { get; set; }
       public Customer Customers { get; set; }
        public Servece Services { get; set; }


    }
}
