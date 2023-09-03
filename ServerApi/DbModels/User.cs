using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ServerApi.DbModels
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
