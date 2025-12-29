namespace Ecommers.Web.ViewComponents
{
    public class LoggedUser
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }

        // Roles del usuario
        public IList<string> Roles { get; set; } = new List<string>();

        // Propiedad de conveniencia para saber si es Admin
        public bool IsAdmin => Roles.Contains("Admin");
    }

}

