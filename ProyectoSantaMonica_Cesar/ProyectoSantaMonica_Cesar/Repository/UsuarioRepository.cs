using Microsoft.Data.SqlClient;
using ProyectoSantaMonica_Cesar.Models;
using System.Data;

namespace ProyectoSantaMonica_Cesar.Repository
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CibertecConnection");
        }

        //  Registrar usuario
        public async Task RegistrarUsuarioAsync(Usuario u)
        {
            var sql = "sp_insertarUsuario";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", u.Username);
                cmd.Parameters.AddWithValue("@Contrasenia", u.Contrasenia);
                cmd.Parameters.AddWithValue("@Nombres", u.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", u.Apellidos);
                cmd.Parameters.AddWithValue("@Dni", u.Dni);
                cmd.Parameters.AddWithValue("@Telefono", (object?)u.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object?)u.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Img_Perfil", (object?)u.Img_Perfil ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rol", u.Rol);

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //  Buscar por Username (login / búsqueda)
        public async Task<Usuario> BuscarPorUsernameAsync(string username)
        {
            Usuario usuario = null;
            var sql = "sp_buscarUsuarioPorUsername";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);

                await cn.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        usuario = new Usuario()
                        {
                            Id_Usuario = dr.GetInt64(0),
                            Username = dr.GetString(1),
                            Contrasenia = dr.GetString(2),
                            Nombres = dr.GetString(3),
                            Apellidos = dr.GetString(4),
                            Dni = dr.GetString(5),
                            Telefono = dr.IsDBNull(6) ? null : dr.GetString(6),
                            Img_Perfil = dr.IsDBNull(7) ? null : dr.GetString(7),
                            Correo = dr.IsDBNull(8) ? null : dr.GetString(8),
                            Rol = Enum.Parse<Roles>(dr.GetString(9))
                        };
                    }
                }
            }
            return usuario;
        }

        //  Buscar por ID (para editar)
        public async Task<Usuario> BuscarPorIdAsync(long id)
        {
            Usuario usuario = null;
            var sql = "sp_UsuarioId";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_Usuario", id);

                await cn.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        usuario = new Usuario()
                        {
                            Id_Usuario = dr.GetInt64(0),
                            Username = dr.GetString(1),
                            Contrasenia = dr.GetString(2),
                            Nombres = dr.GetString(3),
                            Apellidos = dr.GetString(4),
                            Dni = dr.GetString(5),
                            Telefono = dr.IsDBNull(6) ? null : dr.GetString(6),
                            Img_Perfil = dr.IsDBNull(7) ? null : dr.GetString(7),
                            Correo = dr.IsDBNull(8) ? null : dr.GetString(8),
                            Rol = Enum.Parse<Roles>(dr.GetString(9))
                        };
                    }
                }
            }
            return usuario;
        }

        //  Actualizar usuario
        public async Task<string> ActualizarUsuarioAsync(Usuario u)
        {
            var sql = "sp_actualizarUsuario";
            string mensaje = "";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Usuario", u.Id_Usuario);
                cmd.Parameters.AddWithValue("@Username", u.Username);
                cmd.Parameters.AddWithValue("@Contrasenia", (object?)u.Contrasenia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nombres", u.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", u.Apellidos);
                cmd.Parameters.AddWithValue("@Dni", u.Dni);
                cmd.Parameters.AddWithValue("@Telefono", (object?)u.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object?)u.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Img_Perfil", (object?)u.Img_Perfil ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Rol", u.Rol);

                await cn.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();
                mensaje = result?.ToString();
            }

            return mensaje;
        }
    }
}