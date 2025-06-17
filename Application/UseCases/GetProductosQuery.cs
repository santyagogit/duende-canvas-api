using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;

namespace DuendeCanvasAPI.Application.UseCases
{
    public class GetProductosQuery
    {
        private readonly IProductoRepository _productoRepository;
        public GetProductosQuery(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }
        public async Task<IEnumerable<Producto>> ExecuteAsync()
        {
            return await _productoRepository.GetAllAsync();
        }
    }
}
