using System.ComponentModel.DataAnnotations;

namespace AadharVerify.Models
{
    public class Sample
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}
