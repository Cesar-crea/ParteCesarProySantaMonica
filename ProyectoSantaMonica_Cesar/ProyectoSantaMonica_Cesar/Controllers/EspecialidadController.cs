using Microsoft.AspNetCore.Mvc;
using ProyectoSantaMonica_Cesar.Models;
using ProyectoSantaMonica_Cesar.Repository;

namespace ProyectoSantaMonica_Cesar.Controllers
{
    public class EspecialidadController : Controller
    {
        //Llamamos al repositorio
        private readonly EspecialidadRepository especialidadRepo;

        //Instanciamos el repositorio
        public EspecialidadController(EspecialidadRepository especialidadRepo)
        {
            this.especialidadRepo = especialidadRepo;
        }

        //Listar las especialidades

        public async Task<IActionResult> Index()
        {
            var especialidad = await especialidadRepo.ObtenerEspecialidadAsync();
            return View(especialidad);
        }

        //Agregar las especialidades
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Especialidad model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await especialidadRepo.AgregarEspecialidades(model);
            //Agregado para el mensaje del sweet Alert
            TempData["Success"] = "Especialidad registrada correctamente ✅";
            return RedirectToAction(nameof(Index));
        }

        //Editar las especialidades
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var especialidad = await especialidadRepo.ObtenerEspecialidadPorIdAsync(id);

            if (especialidad == null)
                return NotFound();
            return View(especialidad);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Especialidad model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await especialidadRepo.ActualizarEspecialidadAsync(model);
            TempData["Success"] = "Especialidad actualizada correctamente ✏️";
            return RedirectToAction(nameof(Index));

        }
        //Eliminar las especialidades
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var especialidad = await especialidadRepo.ObtenerEspecialidadPorIdAsync(id);
            if (especialidad == null)
                return NotFound();
            return View(especialidad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await especialidadRepo.EliminarEspecialidadAsync(id);

            TempData["Success"] = "Especialidad eliminada correctamente 🗑";

            return RedirectToAction(nameof(Index));
        }




    }
}
