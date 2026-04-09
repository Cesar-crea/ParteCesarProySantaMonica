using Microsoft.Data.SqlClient;
using ProyectoSantaMonica_Cesar.Models;
using System.Data;

namespace ProyectoSantaMonica_Cesar.Repository
{
    public class MedicoRepository
    {
        private readonly string _connectionString;

        public MedicoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        // Listar los medicos
        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            var lista = new List<Medico>();
            var sql = "sp_listarMedico";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Medico()
                        {
                            Id_Medico = dr.GetInt32(0),
                            Nombres = dr.GetString(1),
                            Apellidos = dr.GetString(2),
                            Dni = dr.GetString(3),
                            Nro_Colegiatura = dr.GetString(4),
                            Telefono = dr.IsDBNull(5) ? null : dr.GetString(5),
                            Especialidad = dr.GetString(6) 
                        });
                    }
                }
            }
            return lista;
        }

        // Insertar Medico
        public async Task AgregarMedicoAsync(Medico m)
        {
            var sql = "sp_insertarMedico";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombres", m.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", m.Apellidos);
                cmd.Parameters.AddWithValue("@Dni", m.Dni);
                cmd.Parameters.AddWithValue("@Nro_Colegiatura", m.Nro_Colegiatura);
                cmd.Parameters.AddWithValue("@Telefono", (object?)m.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id_Especialidad", m.Id_Especialidad);

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //Obtener por IdMedico
        public async Task<Medico> ObtenerMedicoPorIdAsync(int id)
        {
            Medico medico = null;
            var sql = "sp_MedicoId";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Medico", id);

                await cn.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        medico = new Medico()
                        {
                            Id_Medico = dr.GetInt32(0),
                            Nombres = dr.GetString(1),
                            Apellidos = dr.GetString(2),
                            Dni = dr.GetString(3),
                            Nro_Colegiatura = dr.GetString(4),
                            Telefono = dr.IsDBNull(5) ? null : dr.GetString(5),
                            Id_Especialidad = dr.GetInt32(6),
                            Especialidad = dr.IsDBNull(7) ? null : dr.GetString(7)
                        };
                    }
                }
            }
            return medico;
        }

        // Actualizar medico
        public async Task ActualizarMedicoAsync(Medico m)
        {
            var sql = "sp_EditarMedico";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Medico", m.Id_Medico);
                cmd.Parameters.AddWithValue("@Nombres", m.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", m.Apellidos);
                cmd.Parameters.AddWithValue("@Dni", m.Dni);
                cmd.Parameters.AddWithValue("@Nro_Colegiatura", m.Nro_Colegiatura);
                cmd.Parameters.AddWithValue("@Telefono", (object?)m.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id_Especialidad", m.Id_Especialidad);

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Eliminar medico
        public async Task EliminarMedicoAsync(int id)
        {
            var sql = "sp_EliminarMedico";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Medico", id);

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //Listar las especialidades 
        //Metodo para listar las especialidades
        public async Task<List<Especialidad>> ObtenerEspecialidadAsync()
        {
            var lista = new List<Especialidad>();
            var sql = "sp_listarEspecialidades";
            using (var cn = new SqlConnection(_connectionString))
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
    }
}