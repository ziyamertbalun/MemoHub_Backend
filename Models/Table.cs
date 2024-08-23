using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MemoHubBackend.Models
{
    public class Table
    {
        [Key]
        public int TableID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; } // Name of the table or category

        [Required]
        public int UserID { get; set; } // Foreign Key to User
        public User? User { get; set; } // Navigation property
        public DateTime CreatedDate { get; set; } // Creation date
        public DateTime? ModifiedDate { get; set; } // Last modified date (optional)
        public ICollection<Todo>? Todos { get; set; } // List of todos in the table

    }
}
