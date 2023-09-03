using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class DishAndDrinkAddOrder
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public float Count { get; set; }
    }
}
