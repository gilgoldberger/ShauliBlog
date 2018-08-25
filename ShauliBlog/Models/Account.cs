using System.ComponentModel.DataAnnotations;

namespace ClassExe3.Models
{
    public class Account
    {
        [Required]
        [Key]
        public string AccountID { get; set; }
        [Required]
        public string Password { get; set; }
    }
}