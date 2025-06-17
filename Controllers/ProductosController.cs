using DuendeCanvasAPI.Application.UseCases;
using DuendeCanvasAPI.Domain.Entities;
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
        public async Task<ActionResult<List<Producto>>> Get()
        {
            try
            {
                var productos = await _getProductosQuery.ExecuteAsync();
                return Ok(productos.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}