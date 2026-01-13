using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;

namespace DuendeCanvasAPI.Application.UseCases
{
    public class GetUltimasEtiquetasQuery
    {
        private readonly IEtiquetaRepository _etiquetaRepository;

        public GetUltimasEtiquetasQuery(IEtiquetaRepository etiquetaRepository)
        {
            _etiquetaRepository = etiquetaRepository;
        }

        public async Task<IEnumerable<Etiqueta>> ExecuteAsync(int limit = 5)
        {
            if (limit <= 0)
            {
                limit = 5;
            }

            return await _etiquetaRepository.GetUltimasAsync(limit);
        }
    }
}
