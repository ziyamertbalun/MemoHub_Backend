using System;

namespace MemoHubBackend.Dtos
{
    public class DescriptionDto
    {
        public int DescriptionID { get; set; }
        public int TodoID { get; set; }
        public string? Content { get; set; }
        public string? ReferencedWord { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
