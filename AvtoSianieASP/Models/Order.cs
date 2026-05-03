using System;
using System.ComponentModel.DataAnnotations;

namespace AvtoSianieASP.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Клиент")]
        public string CustomerId { get; set; } = string.Empty;

        [Display(Name = "Услуга")]
        public int ServeceId { get; set; }

        [Display(Name = "Съобщение")]
        public string Massage { get; set; } = "Няма съобщение";

        [Display(Name = "Дата на създаване")]
        public DateTime DateOn { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата за резервация")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Час")]
        public string ReservationTime { get; set; } = string.Empty;

        public Customer? Customers { get; set; }
        public Servece? Services { get; set; }
    }
}