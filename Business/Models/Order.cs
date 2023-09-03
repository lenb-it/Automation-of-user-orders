using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        public bool IsPaid { get; set; }

        public int NumberPlace { get; set; }

        public DateTime Date { get; set; }
    }
}
