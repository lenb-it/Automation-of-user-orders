namespace Business.Models
{
    public class ChequeDish
    {
        public float Count { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public float TotalPrice => Count * Price;

        public DateTime LastChange { get; set; }

        public int Id { get; set; }
    }
}
