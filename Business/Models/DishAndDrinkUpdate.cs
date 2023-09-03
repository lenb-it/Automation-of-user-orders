using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class DishAndDrinkUpdate
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Id { get; set; }

        public float Price { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        public bool IsValid { get; set; }
    }
}
