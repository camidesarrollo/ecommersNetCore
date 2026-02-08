using Ecommers.Infrastructure.Web.ViewComponents;

namespace Ecommers.Infrastructure.Web.Models.Shared.Components
{
    public class NavbarAdminViewModel
    {
        // Usuario logueado, puede ser null
        public LoggedUser? LoggedUser { get; set; }
        public int NotificationCount { get; set; }
        public List<Notification> Notifications { get; set; } = [];

        public NavbarAdminViewModel()
        {
            // Inicializar con notificaciones de ejemplo
            Notifications =
            [
                new Notification
                {
                    Id = 1,
                    Title = "Nuevo pedido",
                    Message = "Tienes 3 nuevos pedidos pendientes",
                    Time = "Hace 5 minutos",
                    Read = false
                },
                new Notification
                {
                    Id = 2,
                    Title = "Stock bajo",
                    Message = "Almendras naturales están bajo stock",
                    Time = "Hace 1 hora",
                    Read = false
                },
                new Notification
                {
                    Id = 3,
                    Title = "Producto actualizado",
                    Message = "Nueces de California actualizado correctamente",
                    Time = "Hace 2 horas",
                    Read = true
                }
            ];

            NotificationCount = Notifications.Count(n => !n.Read);
        }

       
    }

    public class Notification
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required string Time { get; set; }
        public bool Read { get; set; }
    }
}
