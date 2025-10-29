using DuendeCanvasAPI.Application.UseCases;
using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DuendeCanvasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : Controller
    {
        private readonly GetProductosQuery _getProductosQuery;

        public ProductosController(GetProductosQuery getProductosQuery)
        {
            _getProductosQuery = getProductosQuery;
        }

        [HttpGet]
        public async Task<ActionResult<List<Producto>>> Get(
            [FromQuery] DateTime? fecha = null,
            [FromQuery] char turno = 'M',
            [FromQuery] char operacion = 'N',
            [FromQuery] bool entrada = false)
        {
            try
            {
                var parameters = new ProductoQueryParameters
                {
                    Fecha = fecha,
                    Turno = turno,
                    Operacion = operacion,
                    Entrada = entrada
                };

                var productos = await _getProductosQuery.ExecuteAsync(parameters);
                return Ok(productos.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}