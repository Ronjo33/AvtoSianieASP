using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace AvtoSianieASP.Models
{
    public class Customer : IdentityUser    
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
       
        
        public ICollection<Order> Orders { get; set; }
        
    }
}
