using EmpresaSolutions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmpresaSolutions.Controllers
{
    public class PersonalsController : Controller
    {
        private readonly ContextoSolutions _context;

        public PersonalsController(ContextoSolutions context)
        {
            _context = context;
        }

        // GET: Personals
        public async Task<IActionResult> Index()
        {
            var contextoSolutions = _context.Personales.Include(p => p.Cargo).Include(p => p.Empresa);
            return View(await contextoSolutions.ToListAsync());
        }

        // GET: Personals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Personales == null)
            {
                return NotFound();
            }

            var personal = await _context.Personales
                .Include(p => p.Cargo)
                .Include(p => p.Empresa)
                .FirstOrDefaultAsync(m => m.EmpleadoId == id);
            if (personal == null)
            {
                return NotFound();
            }

            return View(personal);
        }

        // GET: Personals/Create
        public IActionResult Create()
        {
            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "CargoId");
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "EmpresaId");
            return View();
        }

        // POST: Personals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpleadoId,Documento,Nombres,ApellidoPaterno,ApellidoMaterno,FechaNacimiento,FechaIngreso,EmpresaId,CargoId,Active")] Personal personal)
        {
            if (ModelState.IsValid)
                try
                {
                    _context.Add(personal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Manejar o registrar la excepción
                    Console.WriteLine($"Error al guardar datos: {ex.Message}");
                    throw; // O decide cómo manejar la excepción según tus necesidades
                }
            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "CargoId", personal.CargoId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "EmpresaId", personal.EmpresaId);
            return View(personal);
        }

        // GET: Personals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Personales == null)
            {
                return NotFound();
            }

            var personal = await _context.Personales.FindAsync(id);
            if (personal == null)
            {
                return NotFound();
            }
            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "CargoId", personal.CargoId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "EmpresaId", personal.EmpresaId);
            return View(personal);
        }

        // POST: Personals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpleadoId,Documento,Nombres,ApellidoPaterno,ApellidoMaterno,FechaNacimiento,FechaIngreso,EmpresaId,CargoId,Active")] Personal personal)
        {
            if (id != personal.EmpleadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalExists(personal.EmpleadoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CargoId"] = new SelectList(_context.Cargos, "CargoId", "CargoId", personal.CargoId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "EmpresaId", personal.EmpresaId);
            return View(personal);
        }

        // GET: Personals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personales == null)
            {
                return NotFound();
            }

            var personal = await _context.Personales
                .Include(p => p.Cargo)
                .Include(p => p.Empresa)
                .FirstOrDefaultAsync(m => m.EmpleadoId == id);
            if (personal == null)
            {
                return NotFound();
            }

            return View(personal);
        }

        // POST: Personals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personales == null)
            {
                return Problem("Entity set 'ContextoSolutions.Personales'  is null.");
            }
            var personal = await _context.Personales.FindAsync(id);
            if (personal != null)
            {
                _context.Personales.Remove(personal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalExists(int id)
        {
            return (_context.Personales?.Any(e => e.EmpleadoId == id)).GetValueOrDefault();
        }



        public async Task<IActionResult> ExportToExcel()
        {
            var personal = await _context.Personales.ToListAsync();

            var stream = new MemoryStream();
            using (var package = new OfficeOpenXml.ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Personal");
                worksheet.Cells.LoadFromCollection(personal, true);

                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Personal{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
