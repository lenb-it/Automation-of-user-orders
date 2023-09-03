namespace Business.Models
{
    public class Statistics
    {
        public Statistics()
        {
            Dishes = new List<StatisticDish>();
        }

        public List<StatisticDish> Dishes { get; set; }

        public float AverageOrderPrice { get; set; }

        public int CountOrders { get; set; }

        public int CountReservations { get; set; }

        public float MinOrderPrice { get; set; }

        public float MaxOrderPrice { get; set; }
    }
}
