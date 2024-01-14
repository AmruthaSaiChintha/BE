using System.ComponentModel.DataAnnotations;

namespace AadharVerify.Models
{
    public class Phone
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string OTP { get; set; }
    }
}
