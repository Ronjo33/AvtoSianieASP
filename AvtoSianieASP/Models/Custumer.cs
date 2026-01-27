using Microsoft.AspNetCore.Identity;

namespace AvtoSianieASP.Models
{
    public class Custumer : IdentityUser    
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        
        public ICollection<Order> Orders { get; set; }
    }
}
