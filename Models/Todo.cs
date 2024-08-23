using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MemoHubBackend.Models
{
    public class Todo
    {
        [Key]
        public int TodoID { get; set; } // Primary Key

        [Required]
        public int TableID { get; set; } // Foreign Key to Table
        public Table? Table { get; set; } // Navigation property

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; } // Title of the todo

        public string? Content { get; set; } // Detailed content of the todo

        public DateTime? DueDate { get; set; } // Optional due date

        public DateTime? Reminder { get; set; } // Optional reminder date

        public DateTime CreatedDate { get; set; } // Creation date

        public DateTime? ModifiedDate { get; set; } // Last modified date (optional)

        public bool IsCompleted { get; set; } // Status of the todo (completed or not)

        public ICollection<Subnote>? Subnotes { get; set; } // List of subnotes for this todo

        public ICollection<Description>? Descriptions { get; set; } // List of descriptions for this todo
    }
}
