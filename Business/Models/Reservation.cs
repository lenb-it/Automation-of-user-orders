using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class Reservation
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime Date { get; set; }

        [Range(0, int.MaxValue)]
        public int CountPeople { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
