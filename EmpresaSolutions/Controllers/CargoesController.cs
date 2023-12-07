using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpresaSolutions.Models;

namespace EmpresaSolutions.Controllers
{
    public class CargoesController : Controller
    {
        private readonly ContextoSolutions _context;

        public CargoesController(ContextoSolutions context)
        {
            _context = context;
        }

        // GET: Cargoes
        public async Task<IActionResult> Index()
        {
              return _context.Cargos != null ? 
                          View(await _context.Cargos.ToListAsync()) :
                          Problem("Entity set 'ContextoSolutions.Cargos'  is null.");
        }

        // GET: Cargoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cargos == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.CargoId == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // GET: Cargoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cargoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CargoId,DescripcionCargo,FechaCreacion,FlagModificado")] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
        }

        // GET: Cargoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cargos == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }
            return View(cargo);
        }

        // POST: Cargoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CargoId,DescripcionCargo,FechaCreacion,FlagModificado")] Cargo cargo)
        {
            if (id != cargo.CargoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoExists(cargo.CargoId))
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
            return View(cargo);
        }

        // GET: Cargoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cargos == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.CargoId == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // POST: Cargoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cargos == null)
            {
                return Problem("Entity set 'ContextoSolutions.Cargos'  is null.");
            }
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo != null)
            {
                _context.Cargos.Remove(cargo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CargoExists(int id)
        {
          return (_context.Cargos?.Any(e => e.CargoId == id)).GetValueOrDefault();
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var cargos = _context.Cargos.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                cargos = cargos.Where(c => c.DescripcionCargo.Contains(searchString));
            }

            return View(await cargos.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var cargos = await _context.Cargos.ToListAsync();

            var stream = new MemoryStream();
            using (var package = new OfficeOpenXml.ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Cargos");
                worksheet.Cells.LoadFromCollection(cargos, true);

                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Cargos{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

    }
}
