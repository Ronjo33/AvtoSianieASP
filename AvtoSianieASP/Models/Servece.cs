using Microsoft.Extensions.Primitives;
using static AvtoSianieASP.Models.TypesAuto;

namespace AvtoSianieASP.Models
{
    public class Servece
    {
        public int Id { get; set; }
        public string KatNum { get; set; }
        public string DescSurves { get; set; }
        public int CategoryId { get; set; }
        //fk
        public Category Categories { get; set; }
        public  TypeAuto  Equipment { get; set; }
        public int  Duration { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOn { get; set; }

    }
}
