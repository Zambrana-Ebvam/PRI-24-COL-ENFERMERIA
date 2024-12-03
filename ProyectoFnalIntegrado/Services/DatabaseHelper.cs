using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProyectoFnalIntegrado.Data;

namespace ProyectoFnalIntegrado.Services
{
    public class DatabaseHelper
    {
        private readonly DaContext db;

        public DatabaseHelper(DaContext context)
        {
            db = context;
        }

        public async Task<int> GetLastFirstAidKitIdAsync()
        {
            int lastGeneratedId = 0;

            // Crear conexión manualmente usando el DbContext
            var connection = db.Database.GetDbConnection();
            var transaction = await db.Database.BeginTransactionAsync(); // Crear transacción
            try
            {
                // Abrir la conexión si no está abierta
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                // Crear el comando SQL para obtener el último Id de la tabla FirstAidKit
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    SELECT TOP 1 Id
                    FROM FirstAidKit
                    ORDER BY Id DESC";

                    // Asociar la transacción al comando
                    command.Transaction = transaction.GetDbTransaction();  // Asignar transacción

                    // Ejecutar la consulta
                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        lastGeneratedId = Convert.ToInt32(result);
                    }
                }

                // Confirmar la transacción
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // En caso de error, revertir la transacción
                await transaction.RollbackAsync();
                Console.WriteLine($"Error al recuperar el último Id de FirstAidKit: {ex.Message}");
                throw;
            }
            finally
            {
                // Cerrar la conexión si está abierta
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }

            return lastGeneratedId;
        }
    }
}
