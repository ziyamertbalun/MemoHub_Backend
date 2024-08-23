using System;

namespace MemoHubBackend.Dtos
{
    public class SubnoteDto
    {
        public int SubnoteID { get; set; }
        public int TodoID { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
