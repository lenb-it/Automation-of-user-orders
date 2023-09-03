using System.ComponentModel.DataAnnotations;

namespace ServerApi.DbModels
{
    public class DishAndDrink
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int DishAndDrinkCategoryId { get; set; }

        public float Price { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        public bool IsValid { get; set; }

        public DishAndDrinkCategory DishAndDrinkCategory { get; set; }
    }
}
