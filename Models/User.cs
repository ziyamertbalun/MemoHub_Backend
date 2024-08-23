using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MemoHubBackend.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; } // Primary Key
        
        [Required]
        [MaxLength(50)]
        public string? Username  { get; set; } // Username for login
        
        [Required]
        public string? PasswordHash { get; set; } // Hashed password
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; } // User's email address
        public DateTime CreatedDate { get; set; } // Account creation date
    }
}
