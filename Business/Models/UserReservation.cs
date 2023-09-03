using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserReservation
    {
        [Required]
        public string Email { get; set; }

        public DateTime Date { get; set; }

        public int CountPeople { get; set; }
    }
}
