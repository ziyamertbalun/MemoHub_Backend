using System;
using System.Collections.Generic;

namespace MemoHubBackend.Dtos
{
    public class TodoDto
    {
        public int TodoID { get; set; }
        public int TableID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Reminder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsCompleted { get; set; }

        public ICollection<SubnoteDto> Subnotes { get; set; } = new List<SubnoteDto>();
        public ICollection<DescriptionDto> Descriptions { get; set; } = new List<DescriptionDto>();
    }
}
