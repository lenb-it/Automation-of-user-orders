using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class OrderDishAndDrinkUpdate
    {
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        [Required]
        public NewOrderDishAndDrink[] AddDishAndDrinkToOrder { get; set; }
    }
}
