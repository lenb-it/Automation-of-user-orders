using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.DbModels
{
    public class DiscountDishAndDrink
    {
        public int Id { get; set; }
        
        public int DishAndDrinkId { get; set; }

        public float DiscountValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateEnd { get; set; }

        public DateTime ChangeAt { get; set; }

        public DateTime CreateAt { get; set; }

        public DishAndDrink DishAndDrink { get; set; }
    }
}
