using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoSantaMonica_Cesar.Data;
using ProyectoSantaMonica_Cesar.Models;
using System.Data;

namespace ProyectoSantaMonica_Cesar.Repository
{
    public class EspecialidadRepository
    {
        //variable para la cadena de conexion
        private readonly string _stringConnection;

        public EspecialidadRepository(IConfiguration configuration)
        {
            _stringConnection = configuration.GetConnectionString("CibertecConnection");
        }

       

        //Metodo para listar las especialidades
        public async Task<List<Especialidad>> ObtenerEspecialidadAsync()
        {
            var lista = new List<Especialidad>();
            var sql = "sp_listarEspecialidades";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Especialidad()
                        {
                            Id_Especialidad = dr.GetInt32(0),
                            Nombre = dr.IsDBNull(1) ? string.Empty : dr.GetString(1),
                       
                        });
                    }

                }
            }

            return lista;
        }

        //Metodo para agregar Especialidad
        public async Task AgregarEspecialidades(Especialidad e)
        {
            var sql = "sp_insertarEspecialidad";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

        }

        //Obtener producto por Id_Especialidad

        public async Task<Especialidad> ObtenerEspecialidadPorIdAsync(int id)
        {
            Especialidad especialidad = null;
            var sql = "sp_EspecialidadId";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Especialidad", id);
                await cn.OpenAsync();
                using (var dr = cmd.ExecuteReader())
                {
                    if (await dr.ReadAsync())
                    {
                        especialidad = new Especialidad()
                        {
                            Id_Especialidad = dr.GetInt32(0),
                            Nombre = dr.GetString(1)
                         
                        };
                    }

                }
            }
            return especialidad;
        }

        //Actualizar Especialidad

        public async Task ActualizarEspecialidadAsync(Especialidad e)
        {
            var sql = "sp_EditarEspecialidad";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Especialidad", e.Id_Especialidad);
                cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

            }

        }

        //Metodo eliminar_especialidad

        public async Task EliminarEspecialidadAsync(int id)
        {

            var sql = "sp_EliminarCategoria";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Especialidad", id);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

        }




    }
}
