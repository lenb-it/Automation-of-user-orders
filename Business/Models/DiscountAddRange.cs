using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class DiscountAddRange
    {
        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        [Range(1,100)]
        public int Discount { get; set; }

        public int[] DishAndDrinkIds { get; set; }
    }
}
