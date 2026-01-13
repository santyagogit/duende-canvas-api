using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;

namespace DuendeCanvasAPI.Application.UseCases
{
    public class CreateEtiquetaCommand
    {
        private readonly IEtiquetaRepository _etiquetaRepository;

        public CreateEtiquetaCommand(IEtiquetaRepository etiquetaRepository)
        {
            _etiquetaRepository = etiquetaRepository;
        }

        public async Task<Etiqueta> ExecuteAsync(Etiqueta etiqueta)
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

            // Establecer fecha de guardado si no está establecida
            if (etiqueta.FechaGuardado == default)
            {
                etiqueta.FechaGuardado = DateTime.Now;
            }

            return await _etiquetaRepository.CreateAsync(etiqueta);
        }
    }
}
