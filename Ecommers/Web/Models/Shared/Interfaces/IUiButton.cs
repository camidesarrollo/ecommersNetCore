namespace Ecommers.Web.Models.Shared.TagHelpers
{
    public class IUiButton
    {
        public string TipoBoton { get; set; } = "button";
        public string Tipo { get; set; } = "default";
        public string Label { get; set; } = "";
        public string Icono { get; set; } = "user-plus";
        public string IconFamily { get; set; } = "fas";
        public bool Loading { get; set; } = false;
        public string LoadingText { get; set; } = "Procesando...";
        public bool IsFormValid { get; set; } = true;

        /// <summary>
        /// Nombre de una función JS que se ejecutará al hacer click
        /// </summary>
        public required string Accion { get; set; }
    }
}
