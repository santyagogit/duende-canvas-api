using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.DTOs;

namespace DuendeCanvasAPI.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<IEnumerable<Producto>> GetAllAsync(ProductoQueryParameters parameters);
    }
}
