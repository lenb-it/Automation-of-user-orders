using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class NewOrderDishAndDrink
    {
        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public float Count { get; set; }

        [Range(0, int.MaxValue)]
        public int IdOrderDishAndDrink { get; set; }
    }
}
