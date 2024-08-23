namespace MemoHubBackend.Dtos
{
    public class TableDto
    {
        public int TableID { get; set; }
        public string? Name { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
