using Microsoft.AspNetCore.Mvc;
using ProyectoSantaMonica_Cesar.Models;
using ProyectoSantaMonica_Cesar.Repository;

namespace ProyectoSantaMonica_Cesar.Controllers
{
    public class PacienteController : Controller
    {
        //Llamamos al repositorio
        private readonly PacienteRepository pacienteRepo;

        //Instanciamos el repositorio
        public PacienteController(PacienteRepository pacienteRepo)
        {
            this.pacienteRepo = pacienteRepo;
        }

        //Listar lps pacientes

        public async Task<IActionResult> Index()
        {
            var pacientes= await pacienteRepo.ObtenerPacienteAsync();
            return View(pacientes);
        }

        //Agregar los pacientes
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await pacienteRepo.AgregarPacientes(model);
            //Agregado para el mensaje del sweet Alert
            TempData["Success"] = "Paciente registrada correctamente ✅";
            return RedirectToAction(nameof(Index));
        }

        //Editar las especialidades
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var paciente = await pacienteRepo.ObtenerPacientePorIdAsync(id);

            if (paciente == null)
                return NotFound();
            return View(paciente);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Paciente model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await pacienteRepo.ActualizarPacienteAsync(model);
            TempData["Success"] = "Paciente actualizada correctamente ✏️";
            return RedirectToAction(nameof(Index));

        }
        //Eliminar los pacientes
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var paciente = await pacienteRepo.ObtenerPacientePorIdAsync(id);
            if (paciente == null)
                return NotFound();
            return View(paciente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await pacienteRepo.EliminarPacienteAsync(id);

            TempData["Success"] = "Paciente eliminada correctamente 🗑";

            return RedirectToAction(nameof(Index));
        }

    }
}
