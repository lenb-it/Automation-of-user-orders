namespace ServerApi.DbModels
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public float Price { get; set; }

        public bool IsPaid { get; set; }

        public int NumberPlace { get; set; }

        public DateTime ChangeAt { get; set; }

        public DateTime CreateAt { get; set; }

        public User User { get; set; }
    }
}
