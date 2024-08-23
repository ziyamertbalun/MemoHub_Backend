using System;
using System.ComponentModel.DataAnnotations;

namespace MemoHubBackend.Models
{
    public class Description
    {
        [Key]
        public int DescriptionID { get; set; } // Primary Key

        [Required]
        public int TodoID { get; set; } // Foreign Key to Todo
        public Todo? Todo { get; set; } // Navigation property

        [Required]
        public string? Content { get; set; } // Detailed content describing the todo

        [MaxLength(50)]
        public string? ReferencedWord { get; set; }
        public DateTime CreatedDate { get; set; } // Creation date
    }
}
