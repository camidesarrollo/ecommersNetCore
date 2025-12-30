namespace Ecommers.Web.Models
{
    public class MenuItem
    {
        public required string Name { get; set; }
        public required string Path { get; set; }
        public string? Badge { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
