using DuendeCanvasAPI.Domain.Entities;

namespace DuendeCanvasAPI.Domain.Interfaces
{
    public interface IEtiquetaRepository
    {
        Task<Etiqueta> CreateAsync(Etiqueta etiqueta);
        Task<Etiqueta?> GetByIdAsync(int id);
        Task<IEnumerable<Etiqueta>> GetAllAsync();
        Task<IEnumerable<Etiqueta>> GetUltimasAsync(int limit);
        Task<bool> VerificarNombreExistenteAsync(string nombre);
        Task<Etiqueta> UpdateAsync(int id, Etiqueta etiqueta);
    }
}
