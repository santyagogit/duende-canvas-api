using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DuendeCanvasAPI.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            var productos = new List<Producto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ObtenerProductos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync(); // Use await to open the connection asynchronously
                    using (var reader = await command.ExecuteReaderAsync()) // Use await to execute the reader asynchronously
                    {
                        while (await reader.ReadAsync()) // Use await to read asynchronously
                        {
                            productos.Add(new Producto
                            {
                                Id = reader["Id"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"])
                            });
                        }
                    }
                }
            }

            return productos;
        }
    }
}
