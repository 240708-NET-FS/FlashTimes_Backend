using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlashTimes.Models;

public class Users
{         
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }=string.Empty; 

        
}