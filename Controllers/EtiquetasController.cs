using DuendeCanvasAPI.Application.UseCases;
using DuendeCanvasAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DuendeCanvasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtiquetasController : Controller
    {
        private readonly CreateEtiquetaCommand _createEtiquetaCommand;
        private readonly GetEtiquetaByIdQuery _getEtiquetaByIdQuery;
        private readonly GetUltimasEtiquetasQuery _getUltimasEtiquetasQuery;
        private readonly VerificarNombreEtiquetaQuery _verificarNombreEtiquetaQuery;
        private readonly UpdateEtiquetaCommand _updateEtiquetaCommand;

        public EtiquetasController(
            CreateEtiquetaCommand createEtiquetaCommand,
            GetEtiquetaByIdQuery getEtiquetaByIdQuery,
            GetUltimasEtiquetasQuery getUltimasEtiquetasQuery,
            VerificarNombreEtiquetaQuery verificarNombreEtiquetaQuery,
            UpdateEtiquetaCommand updateEtiquetaCommand)
        {
            _createEtiquetaCommand = createEtiquetaCommand;
            _getEtiquetaByIdQuery = getEtiquetaByIdQuery;
            _getUltimasEtiquetasQuery = getUltimasEtiquetasQuery;
            _verificarNombreEtiquetaQuery = verificarNombreEtiquetaQuery;
            _updateEtiquetaCommand = updateEtiquetaCommand;
        }

        [HttpPost]
        public async Task<ActionResult<Etiqueta>> Post([FromBody] CreateEtiquetaRequest request)
        {
            try
            {
                // Convertir objects a JSON string si viene como objeto
                string objectsJson = string.Empty;
                if (request.Objects != null)
                {
                    if (request.Objects is string)
                    {
                        objectsJson = (string)request.Objects;
                    }
                    else
                    {
                        objectsJson = JsonSerializer.Serialize(request.Objects);
                    }
                }

                var etiqueta = new Etiqueta
                {
                    Nombre = request.Nombre,
                    Width = request.Width,
                    Height = request.Height,
                    Objects = objectsJson,
                    FechaGuardado = request.FechaGuardado != default ? request.FechaGuardado : DateTime.Now
                };

                var resultado = await _createEtiquetaCommand.ExecuteAsync(etiqueta);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(int id)
        {
            try
            {
                var etiqueta = await _getEtiquetaByIdQuery.ExecuteAsync(id);
                if (etiqueta == null)
                {
                    return NotFound($"No se encontró la etiqueta con ID {id}");
                }

                // Parsear Objects JSON para devolverlo como objeto
                object? objectsParsed = null;
                if (!string.IsNullOrEmpty(etiqueta.Objects))
                {
                    try
                    {
                        objectsParsed = JsonSerializer.Deserialize<object>(etiqueta.Objects);
                    }
                    catch
                    {
                        // Si falla el parseo, devolver el string original
                        objectsParsed = etiqueta.Objects;
                    }
                }

                var etiquetaResponse = new
                {
                    id = etiqueta.Id,
                    nombre = etiqueta.Nombre,
                    width = etiqueta.Width,
                    height = etiqueta.Height,
                    objects = objectsParsed,
                    fechaGuardado = etiqueta.FechaGuardado
                };

                return Ok(etiquetaResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("ultimas")]
        public async Task<ActionResult<List<object>>> GetUltimas([FromQuery] int limit = 5)
        {
            try
            {
                var etiquetas = await _getUltimasEtiquetasQuery.ExecuteAsync(limit);
                
                // Retornar solo los campos necesarios para la lista (sin Objects completo)
                var etiquetasLista = etiquetas.Select(e => new
                {
                    id = e.Id,
                    nombre = e.Nombre,
                    width = e.Width,
                    height = e.Height,
                    fechaGuardado = e.FechaGuardado
                }).ToList();

                return Ok(etiquetasLista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("verificar-nombre/{nombre}")]
        public async Task<ActionResult<bool>> VerificarNombre(string nombre)
        {
            try
            {
                var existe = await _verificarNombreEtiquetaQuery.ExecuteAsync(nombre);
                return Ok(existe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Etiqueta>> Put(int id, [FromBody] UpdateEtiquetaRequest request)
        {
            try
            {
                // Convertir objects a JSON string si viene como objeto
                string objectsJson = string.Empty;
                if (request.Objects != null)
                {
                    if (request.Objects is string)
                    {
                        objectsJson = (string)request.Objects;
                    }
                    else
                    {
                        objectsJson = JsonSerializer.Serialize(request.Objects);
                    }
                }

                var etiqueta = new Etiqueta
                {
                    Nombre = request.Nombre,
                    Width = request.Width,
                    Height = request.Height,
                    Objects = objectsJson,
                    FechaGuardado = request.FechaGuardado != default ? request.FechaGuardado : DateTime.Now
                };

                var resultado = await _updateEtiquetaCommand.ExecuteAsync(id, etiqueta);
                return Ok(resultado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }

    // DTOs para requests
    public class CreateEtiquetaRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public object? Objects { get; set; }
        public DateTime FechaGuardado { get; set; }
    }

    public class UpdateEtiquetaRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public object? Objects { get; set; }
        public DateTime FechaGuardado { get; set; }
    }
}
