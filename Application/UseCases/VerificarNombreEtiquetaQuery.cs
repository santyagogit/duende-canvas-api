using DuendeCanvasAPI.Domain.Interfaces;

namespace DuendeCanvasAPI.Application.UseCases
{
    public class VerificarNombreEtiquetaQuery
    {
        private readonly IEtiquetaRepository _etiquetaRepository;

        public VerificarNombreEtiquetaQuery(IEtiquetaRepository etiquetaRepository)
        {
            _etiquetaRepository = etiquetaRepository;
        }

        public async Task<bool> ExecuteAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return false;
            }

            return await _etiquetaRepository.VerificarNombreExistenteAsync(nombre);
        }
    }
}
