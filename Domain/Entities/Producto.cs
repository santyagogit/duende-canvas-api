namespace DuendeCanvasAPI.Domain.Entities
{
    public class Producto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; } = 0;
    }
}
