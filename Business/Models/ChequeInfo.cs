namespace Business.Models
{
    public class ChequeInfo
    {
        public DateTime Date { get; set; }

        public float TotalPrice { get; set; }

        public List<ChequeDish> ChequeDishes { get; set; }
    }
}
