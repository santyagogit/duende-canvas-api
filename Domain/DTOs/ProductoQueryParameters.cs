namespace DuendeCanvasAPI.Domain.DTOs
{
    public class ProductoQueryParameters
    {
        public DateTime? Fecha { get; set; }
        public char Turno { get; set; } = 'M';
        public char Operacion { get; set; } = 'N';
    }
}
