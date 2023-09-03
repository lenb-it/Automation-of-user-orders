using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class DishAndDrinkMenu
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public float Price { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public bool Discount { get; set; }

        public bool IsValid { get; set; }
    }
}
