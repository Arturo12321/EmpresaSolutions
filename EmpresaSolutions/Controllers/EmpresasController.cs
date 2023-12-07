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
    public class EmpresasController : Controller
    {
        private readonly ContextoSolutions _context;

        public EmpresasController(ContextoSolutions context)
        {
            _context = context;
        }

        // GET: Empresas
        public async Task<IActionResult> Index()
        {
              return _context.Empresas != null ? 
                          View(await _context.Empresas.ToListAsync()) :
                          Problem("Entity set 'ContextoSolutions.Empresas'  is null.");
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.EmpresaId == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpresaId,RUC,RazonSocial,FechaFundacion,Estado")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpresaId,RUC,RazonSocial,FechaFundacion,Estado")] Empresa empresa)
        {
            if (id != empresa.EmpresaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.EmpresaId))
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
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.EmpresaId == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'ContextoSolutions.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
          return (_context.Empresas?.Any(e => e.EmpresaId == id)).GetValueOrDefault();
        }
        [HttpGet]
        public async Task<IActionResult> IndexEmpresa(int? ruc)
        {
            var empresas = _context.Empresas.AsQueryable();

            if (ruc.HasValue)
            {
                empresas = empresas.Where(e => e.RUC == ruc);
            }

            return View(await empresas.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> IndexEmpresa2(string razonSocial)
        {
            var empresas = _context.Empresas.AsQueryable();

            if (!string.IsNullOrEmpty(razonSocial))
            {
                empresas = empresas.Where(e => e.RazonSocial.Contains(razonSocial));
            }

            return View(await empresas.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var empresas = await _context.Empresas.ToListAsync();

            var stream = new MemoryStream();
            using (var package = new OfficeOpenXml.ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Empresas");
                worksheet.Cells.LoadFromCollection(empresas, true);

                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Empresas_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
