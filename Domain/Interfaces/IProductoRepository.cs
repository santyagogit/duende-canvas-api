using DuendeCanvasAPI.Domain.Entities;

namespace DuendeCanvasAPI.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
    }
}
