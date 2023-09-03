namespace ServerApi.DbModels
{
    public class OrderDishAndDrink
    {
        public int Id { get; set; }

        public int DishAndDrinkId { get; set; }

        public int OrderId { get; set; }

        public float Count { get; set; }

        public DateTime ChangeAt { get; set; }

        public DateTime CreateAt { get; set; }

        public Order Order { get; set; }

        public DishAndDrink DishAndDrink { get; set; }
    }
}
