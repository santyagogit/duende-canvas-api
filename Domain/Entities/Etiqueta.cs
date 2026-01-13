namespace DuendeCanvasAPI.Domain.Entities
{
    public class Etiqueta
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Objects { get; set; } = string.Empty; // JSON string del canvas
        public DateTime FechaGuardado { get; set; }
    }
}
