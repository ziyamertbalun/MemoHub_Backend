namespace MemoHubBackend.Dtos
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
