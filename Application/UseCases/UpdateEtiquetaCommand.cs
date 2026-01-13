using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;

namespace DuendeCanvasAPI.Application.UseCases
{
    public class UpdateEtiquetaCommand
    {
        private readonly IEtiquetaRepository _etiquetaRepository;

        public UpdateEtiquetaCommand(IEtiquetaRepository etiquetaRepository)
        {
            _etiquetaRepository = etiquetaRepository;
        }

        public async Task<Etiqueta> ExecuteAsync(int id, Etiqueta etiqueta)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(etiqueta.Nombre))
            {
                throw new ArgumentException("El nombre de la etiqueta no puede estar vacío");
            }

            if (etiqueta.Width <= 0 || etiqueta.Height <= 0)
            {
                throw new ArgumentException("El ancho y alto deben ser mayores a 0");
            }

            // Verificar que la etiqueta existe
            var etiquetaExistente = await _etiquetaRepository.GetByIdAsync(id);
            if (etiquetaExistente == null)
            {
                throw new KeyNotFoundException($"No se encontró la etiqueta con ID {id}");
            }

            // Mantener la fecha original si no se especifica una nueva
            if (etiqueta.FechaGuardado == default)
            {
                etiqueta.FechaGuardado = etiquetaExistente.FechaGuardado;
            }

            return await _etiquetaRepository.UpdateAsync(id, etiqueta);
        }
    }
}
