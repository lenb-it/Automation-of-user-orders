namespace Business.Models
{
    public class DiscountViewModel
    {
        public int Id { get; set; }

        public string DishAndDrinkName { get; set; }

        public float DiscountValue { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public float DiscountPrice { get; set; }
    }
}
