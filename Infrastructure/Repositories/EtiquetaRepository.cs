using DuendeCanvasAPI.Domain.Entities;
using DuendeCanvasAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DuendeCanvasAPI.Infrastructure.Repositories
{
    public class EtiquetaRepository : IEtiquetaRepository
    {
        private readonly string _connectionString;

        public EtiquetaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Etiqueta> CreateAsync(Etiqueta etiqueta)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
                        INSERT INTO Etiquetas (Nombre, Width, Height, Objects, FechaGuardado)
                        OUTPUT INSERTED.Id, INSERTED.Nombre, INSERTED.Width, INSERTED.Height, INSERTED.Objects, INSERTED.FechaGuardado
                        VALUES (@Nombre, @Width, @Height, @Objects, @FechaGuardado)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar) { Value = etiqueta.Nombre });
                        command.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int) { Value = etiqueta.Width });
                        command.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int) { Value = etiqueta.Height });
                        command.Parameters.Add(new SqlParameter("@Objects", SqlDbType.NVarChar, -1) { Value = etiqueta.Objects ?? string.Empty });
                        command.Parameters.Add(new SqlParameter("@FechaGuardado", SqlDbType.DateTime) { Value = etiqueta.FechaGuardado });

                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Etiqueta
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Width = reader.GetInt32("Width"),
                                    Height = reader.GetInt32("Height"),
                                    Objects = reader.GetString("Objects"),
                                    FechaGuardado = reader.GetDateTime("FechaGuardado")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear etiqueta: {ex.Message}", ex);
            }

            throw new Exception("No se pudo crear la etiqueta");
        }

        public async Task<Etiqueta?> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT Id, Nombre, Width, Height, Objects, FechaGuardado FROM Etiquetas WHERE Id = @Id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Etiqueta
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Width = reader.GetInt32("Width"),
                                    Height = reader.GetInt32("Height"),
                                    Objects = reader.GetString("Objects"),
                                    FechaGuardado = reader.GetDateTime("FechaGuardado")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener etiqueta: {ex.Message}", ex);
            }

            return null;
        }

        public async Task<IEnumerable<Etiqueta>> GetAllAsync()
        {
            var etiquetas = new List<Etiqueta>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT Id, Nombre, Width, Height, Objects, FechaGuardado FROM Etiquetas ORDER BY FechaGuardado DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                etiquetas.Add(new Etiqueta
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Width = reader.GetInt32("Width"),
                                    Height = reader.GetInt32("Height"),
                                    Objects = reader.GetString("Objects"),
                                    FechaGuardado = reader.GetDateTime("FechaGuardado")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener etiquetas: {ex.Message}", ex);
            }

            return etiquetas;
        }

        public async Task<IEnumerable<Etiqueta>> GetUltimasAsync(int limit)
        {
            var etiquetas = new List<Etiqueta>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
                        SELECT TOP (@Limit) Id, Nombre, Width, Height, Objects, FechaGuardado 
                        FROM Etiquetas 
                        ORDER BY FechaGuardado DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Limit", SqlDbType.Int) { Value = limit });

                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                etiquetas.Add(new Etiqueta
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Width = reader.GetInt32("Width"),
                                    Height = reader.GetInt32("Height"),
                                    Objects = reader.GetString("Objects"),
                                    FechaGuardado = reader.GetDateTime("FechaGuardado")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener últimas etiquetas: {ex.Message}", ex);
            }

            return etiquetas;
        }

        public async Task<bool> VerificarNombreExistenteAsync(string nombre)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT COUNT(1) FROM Etiquetas WHERE Nombre = @Nombre";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar) { Value = nombre });

                        await connection.OpenAsync();
                        var count = (int)await command.ExecuteScalarAsync();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar nombre: {ex.Message}", ex);
            }
        }

        public async Task<Etiqueta> UpdateAsync(int id, Etiqueta etiqueta)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
                        UPDATE Etiquetas 
                        SET Nombre = @Nombre, Width = @Width, Height = @Height, Objects = @Objects, FechaGuardado = @FechaGuardado
                        WHERE Id = @Id
                        SELECT Id, Nombre, Width, Height, Objects, FechaGuardado FROM Etiquetas WHERE Id = @Id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
                        command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar) { Value = etiqueta.Nombre });
                        command.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int) { Value = etiqueta.Width });
                        command.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int) { Value = etiqueta.Height });
                        command.Parameters.Add(new SqlParameter("@Objects", SqlDbType.NVarChar, -1) { Value = etiqueta.Objects ?? string.Empty });
                        command.Parameters.Add(new SqlParameter("@FechaGuardado", SqlDbType.DateTime) { Value = etiqueta.FechaGuardado });

                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Etiqueta
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Width = reader.GetInt32("Width"),
                                    Height = reader.GetInt32("Height"),
                                    Objects = reader.GetString("Objects"),
                                    FechaGuardado = reader.GetDateTime("FechaGuardado")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar etiqueta: {ex.Message}", ex);
            }

            throw new Exception("No se pudo actualizar la etiqueta");
        }
    }
}
