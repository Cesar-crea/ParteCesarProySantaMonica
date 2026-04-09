using Microsoft.Data.SqlClient;
using ProyectoSantaMonica_Cesar.Models;
using System.Data;

namespace ProyectoSantaMonica_Cesar.Repository
{
    public class PacienteRepository
    {
        //variable para la cadena de conexion
        private readonly string _stringConnection;

        public PacienteRepository(IConfiguration configuration)
        {
            _stringConnection = configuration.GetConnectionString("CibertecConnection");
        }



        //Metodo para listar los pacientes
        public async Task<List<Paciente>> ObtenerPacienteAsync()
        {
            var lista = new List<Paciente>();
            var sql = "sp_listarPaciente";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Paciente()
                        {
                            Id_Paciente = dr.GetInt32(0),
                            Nombres = dr.IsDBNull(1) ? string.Empty : dr.GetString(1),
                            Apellidos=dr.GetString(2),
                            Dni = dr.GetString(3),
                            Fecha_Nacimiento = dr.GetDateTime(4),
                            Telefono =dr.GetString(5)
                        });
                    }

                }
            }

            return lista;
        }

        //Metodo para agregar Paciente
        public async Task AgregarPacientes(Paciente p)
        {
            var sql = "sp_insertarPaciente";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombres", p.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", p.Apellidos);
                cmd.Parameters.AddWithValue("@Dni", p.Dni);
                cmd.Parameters.AddWithValue("@Fecha_Nacimiento", p.Fecha_Nacimiento);
                cmd.Parameters.AddWithValue("@Telefono", p.Telefono);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

        }

        //Obtener producto por Id_Paciente

        public async Task<Paciente> ObtenerPacientePorIdAsync(int id)
        {
            Paciente paciente = null;
            var sql = "sp_PacienteId";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Paciente", id);
                await cn.OpenAsync();
                using (var dr = cmd.ExecuteReader())
                {
                    if (await dr.ReadAsync())
                    {
                        paciente= new Paciente()
                        {
                            Id_Paciente = dr.GetInt32(0),
                            Nombres = dr.IsDBNull(1) ? string.Empty : dr.GetString(1),
                            Apellidos = dr.GetString(2),
                            Dni = dr.GetString(3),
                            Fecha_Nacimiento = dr.GetDateTime(4),
                            Telefono = dr.GetString(5)

                        };
                    }

                }
            }
            return paciente;
        }

        //Actualizar Paciente

        public async Task ActualizarPacienteAsync(Paciente p)
        {
            var sql = "sp_EditarPaciente";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Paciente", p.Id_Paciente);
                cmd.Parameters.AddWithValue("@Nombres", p.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", p.Apellidos);
                cmd.Parameters.AddWithValue("@Dni", p.Dni);
                cmd.Parameters.AddWithValue("@Fecha_Nacimiento", p.Fecha_Nacimiento);
                cmd.Parameters.AddWithValue("@Telefono", p.Telefono);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

            }

        }

        //Metodo eliminar_paciente

        public async Task EliminarPacienteAsync(int id)
        {

            var sql = "sp_EliminarPaciente";
            using (var cn = new SqlConnection(_stringConnection))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Paciente", id);
                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

        }


    }
}
