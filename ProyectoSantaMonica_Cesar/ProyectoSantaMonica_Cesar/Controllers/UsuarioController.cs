using Microsoft.AspNetCore.Mvc;
using ProyectoSantaMonica_Cesar.Models;
using ProyectoSantaMonica_Cesar.Repository;

namespace ProyectoSantaMonica_Cesar.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository usuarioRepo;

        public UsuarioController(UsuarioRepository usuarioRepo)
        {
            this.usuarioRepo = usuarioRepo;
        }

        // HOME
        public IActionResult Index()
        {
            return View();
        }

        // LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // PROCESAR LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var usuario = await usuarioRepo.BuscarPorUsernameAsync(username);

            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToAction(nameof(Login));
            }

            // 🔥 VALIDACIÓN CLAVE
            if (string.IsNullOrEmpty(usuario.Contrasenia))
            {
                TempData["Error"] = "El usuario no tiene contraseña registrada";
                return RedirectToAction(nameof(Login));
            }

            if (string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Ingrese la contraseña";
                return RedirectToAction(nameof(Login));
            }

            bool esValido = BCrypt.Net.BCrypt.Verify(password, usuario.Contrasenia);

            if (!esValido)
            {
                TempData["Error"] = "Contraseña incorrecta";
                return RedirectToAction(nameof(Login));
            }

            HttpContext.Session.SetString("Usuario", usuario.Username);
            HttpContext.Session.SetString("Rol", usuario.Rol.ToString());

            TempData["Success"] = "Bienvenido " + usuario.Username;

            if (usuario.Rol.ToString() == "ADMINISTRADOR")
                return RedirectToAction(nameof(AdminHome));

            if (usuario.Rol.ToString() == "RECEPCIONISTA")
                return RedirectToAction(nameof(RecepcionistaHome));

            if (usuario.Rol.ToString() == "CAJERO")
                return RedirectToAction(nameof(CajeroHome));

            return RedirectToAction(nameof(Login));
        }

        // VISTAS POR ROL
        public IActionResult AdminHome()
        {
            return View("Admin/admin");
        }

        public IActionResult RecepcionistaHome()
        {
            return View("Recepcionista/recepcionista");
        }

        public IActionResult CajeroHome()
        {
            return View("Cajero/cajero");
        }

        // REGISTRO
        [HttpGet]
        public IActionResult Registro()
        {
            return View("Admin/registro", new Usuario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Usuario model, IFormFile archivoImagen)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Admin/registro", model);

                // Imagen
                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var nombreArchivo = DateTime.Now.Ticks + "_" + archivoImagen.FileName;
                    var ruta = Path.Combine(folder, nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await archivoImagen.CopyToAsync(stream);
                    }

                    model.Img_Perfil = "/uploads/" + nombreArchivo;
                }
                else
                {
                    model.Img_Perfil = "/images/default-user.jpg";
                }

                // HASH PASSWORD
                model.Contrasenia = BCrypt.Net.BCrypt.HashPassword(model.Contrasenia);

                await usuarioRepo.RegistrarUsuarioAsync(model);

                TempData["Success"] = "Usuario registrado correctamente ✅";
                return RedirectToAction(nameof(AdminHome));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al registrar usuario ❌";
                return View("Admin/registro", model);
            }
        }

        // EDITAR (buscar)
        [HttpGet]
        public async Task<IActionResult> Editar(string username)
        {
            if (string.IsNullOrEmpty(username))
                return View("Admin/editar");

            var usuario = await usuarioRepo.BuscarPorUsernameAsync(username);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario no encontrado ❌";
                return View("Admin/editar");
            }

            return View("Admin/editar", usuario);
        }

        // ACTUALIZAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(Usuario model, IFormFile archivoImagen)
        {
            try
            {
                var usuarioBD = await usuarioRepo.BuscarPorIdAsync(model.Id_Usuario);

                if (usuarioBD == null)
                {
                    TempData["Error"] = "Usuario no encontrado ❌";
                    return View("Admin/editar", model);
                }

                // Imagen
                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var nombreArchivo = DateTime.Now.Ticks + "_" + archivoImagen.FileName;
                    var ruta = Path.Combine(folder, nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await archivoImagen.CopyToAsync(stream);
                    }

                    model.Img_Perfil = "/uploads/" + nombreArchivo;
                }
                else
                {
                    model.Img_Perfil = usuarioBD.Img_Perfil;
                }

                // Password opcional
                if (!string.IsNullOrEmpty(model.Contrasenia))
                {
                    model.Contrasenia = BCrypt.Net.BCrypt.HashPassword(model.Contrasenia);
                }
                else
                {
                    model.Contrasenia = usuarioBD.Contrasenia;
                }

                var mensaje = await usuarioRepo.ActualizarUsuarioAsync(model);

                if (mensaje == "OK")
                {
                    TempData["Success"] = "Usuario actualizado correctamente ✏️";
                }
                else
                {
                    TempData["Error"] = mensaje;
                }

                // 
                return RedirectToAction(nameof(Editar), new { username = model.Username });
            }
            catch (Exception)
            {
                TempData["Error"] = "Error al actualizar ❌";
                return RedirectToAction(nameof(Editar), new { username = model.Username });
            }
        }
    }
}