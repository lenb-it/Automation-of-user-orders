using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class OrderAdd
    {
        [Required]
        public string Email { get; set; }

        [Range(0, int.MaxValue)]
        public int NumberPlace { get; set; }

        public List<DishAndDrinkAddOrder> DishAndDrinks { get; set; }
    }
}
