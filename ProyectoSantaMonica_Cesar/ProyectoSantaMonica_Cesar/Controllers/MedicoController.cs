using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoSantaMonica_Cesar.Models;
using ProyectoSantaMonica_Cesar.Repository;

namespace ProyectoSantaMonica_Cesar.Controllers
{
    public class MedicoController : Controller
    {
        // Repositorio
        private readonly MedicoRepository medicoRepo;

        
        public MedicoController(MedicoRepository medicoRepo)
        {
            this.medicoRepo = medicoRepo;
        }

        //Listar Medicos
        public async Task<IActionResult> Index()
        {
            var medicos = await medicoRepo.ObtenerMedicosAsync();
            return View(medicos);
        }

        // Para registrar Medicos
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarEspecialidades();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medico model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await medicoRepo.AgregarMedicoAsync(model);

            TempData["Success"] = "Médico registrado correctamente 👨‍⚕️";
            return RedirectToAction(nameof(Index));
        }

        // Para editar el Medico
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var medico = await medicoRepo.ObtenerMedicoPorIdAsync(id);
            await CargarEspecialidades();
            return View(medico);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Medico model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await medicoRepo.ActualizarMedicoAsync(model);

            TempData["Success"] = "Médico actualizado correctamente ✏️";
            return RedirectToAction(nameof(Index));
        }

        // Para eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var medico = await medicoRepo.ObtenerMedicoPorIdAsync(id);

            if (medico == null)
                return NotFound();

            return View(medico);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await medicoRepo.EliminarMedicoAsync(id);

            TempData["Success"] = "Médico eliminado correctamente 🗑";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarEspecialidades()
        {
            var lista = await medicoRepo.ObtenerEspecialidadAsync();

            ViewBag.Especialidades = lista.Select(e => new SelectListItem
            {
                Value = e.Id_Especialidad.ToString(),
                Text = e.Nombre
            }).ToList();
        }
    }
}