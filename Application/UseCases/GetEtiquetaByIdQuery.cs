using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;

namespace DuendeCanvasAPI.Application.UseCases
{
    public class GetEtiquetaByIdQuery
    {
        private readonly IEtiquetaRepository _etiquetaRepository;

        public GetEtiquetaByIdQuery(IEtiquetaRepository etiquetaRepository)
        {
            _etiquetaRepository = etiquetaRepository;
        }

        public async Task<Etiqueta?> ExecuteAsync(int id)
        {
            return await _etiquetaRepository.GetByIdAsync(id);
        }
    }
}
