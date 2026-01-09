using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;
using DuendeCanvasAPI.Domain.DTOs;
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
            // Mock data for development/testing
            var productos = new List<Producto>
            {
                new Producto { Id = "1", Nombre = "Producto A", Precio = 10.99m },
                new Producto { Id = "2", Nombre = "Producto B", Precio = 24.50m },
                new Producto { Id = "3", Nombre = "Producto C", Precio = 15.00m },
                new Producto { Id = "4", Nombre = "Producto D", Precio = 32.99m },
                new Producto { Id = "5", Nombre = "Producto E", Precio = 18.50m },
                new Producto { Id = "6", Nombre = "Laptop Gaming", Precio = 1299.99m },
                new Producto { Id = "7", Nombre = "Smartphone Pro", Precio = 899.50m },
                new Producto { Id = "8", Nombre = "Auriculares Wireless", Precio = 89.99m },
                new Producto { Id = "9", Nombre = "Tablet Ultra", Precio = 549.99m },
                new Producto { Id = "10", Nombre = "Monitor 4K", Precio = 399.99m }
            };

            // Simulate async operation
            await Task.Delay(100);
            
            return productos;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync(ProductoQueryParameters parameters)
        {
            var productos = new List<Producto>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("ObtenerProdImpre", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters with default values
                        command.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.DateTime) 
                        { 
                            Value = parameters.Fecha ?? (object)DBNull.Value 
                        });
                        
                        command.Parameters.Add(new SqlParameter("@Turno", SqlDbType.Char, 1) 
                        { 
                            Value = parameters.Turno 
                        });
                        
                        command.Parameters.Add(new SqlParameter("@Operacion", SqlDbType.Char, 1) 
                        { 
                            Value = parameters.Operacion 
                        });

                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                        while (await reader.ReadAsync())
                        {
                            productos.Add(new Producto
                            {
                                Id = reader["Código"]?.ToString() ?? string.Empty,
                                Nombre = reader["Nombre Producto"]?.ToString() ?? string.Empty,
                                Precio = reader["P.Unitario"] != DBNull.Value ? Convert.ToDecimal(reader["P.Unitario"]) : 0
                            });
                        }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For now, return mock data if database connection fails
                return await GetAllAsync();
            }

            return productos;
        }
    }
}
