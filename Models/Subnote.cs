using System;
using System.ComponentModel.DataAnnotations;

namespace MemoHubBackend.Models
{
    public class Subnote
    {
        [Key]
        public int SubnoteID { get; set; } // Primary Key

        [Required]
        public int TodoID { get; set; } // Foreign Key to Todo
        public Todo? Todo { get; set; } // Navigation property

        [Required]
        [MaxLength(200)]
        public string? Content { get; set; } // Content of the subnote
        public DateTime? Reminder { get; set; } // Optional reminder date
        public DateTime CreatedDate { get; set; } // Creation date
    }
}
