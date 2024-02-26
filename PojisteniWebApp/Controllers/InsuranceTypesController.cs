using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Classes;
using PojisteniWebApp.Data;
using PojisteniWebApp.Models;

namespace PojisteniWebApp.Controllers
{
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Insured}")]
    public class InsuranceTypesController(ApplicationDbContext _dbContext) : Controller
    {
        private readonly ApplicationDbContext _dbContext = _dbContext;

        // GET: InsuranceTypes
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.InsuranceTypes.ToListAsync());
        }

        // GET: InsuranceTypes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insuranceType = await _dbContext.InsuranceTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceType == null)
            {
                return NotFound();
            }
            return View(insuranceType);
        }

        // GET: InsuranceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] InsuranceType insuranceType)
        {
            //ukontrolujeme jestli uz dany produkt existuje
            if (InsuranceTypeExists(insuranceType.Title))
            {
                TempData["error"] = "Tento produkt již existuje";
                return View(insuranceType);
            }
            if (ModelState.IsValid)
            {
                _dbContext.Add(insuranceType);
                await _dbContext.SaveChangesAsync();
                TempData["success"] = "Pojištění úspěšne vytvořeno";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Chyba při vytváření pojištění";
            return View(insuranceType);
        }

        // GET: InsuranceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insuranceType = await _dbContext.InsuranceTypes.FindAsync(id);
            if (insuranceType == null)
            {
                return NotFound();
            }
            return View(insuranceType);
        }

        // POST: InsuranceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] InsuranceType insuranceType)
        {
            if (id != insuranceType.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(insuranceType);
					await _dbContext.SaveChangesAsync();
					TempData["success"] = "Pojištění úspěšne upraveno";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceTypeExists(insuranceType.Id))
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
            TempData["error"] = "Chyba upravení pojištění";
            return View(insuranceType);
        }

        // GET: InsuranceTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insuranceType = await _dbContext.InsuranceTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceType == null)
            {
                return NotFound();
            }
            return View(insuranceType);
        }

        // POST: InsuranceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuranceType = await _dbContext.InsuranceTypes.FindAsync(id);
            if (insuranceType != null)
            {
                _dbContext.InsuranceTypes.Remove(insuranceType);
            }
            await _dbContext.SaveChangesAsync();
			TempData["success"] = "Pojištění úspěšne smazáno";
			return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// zjistí jestli uz produkt existuje
        /// </summary>
        /// <param name="id">číslo id</param>
        /// <returns>logickou hodnotu</returns>
        private bool InsuranceTypeExists(int id)
        {
            return _dbContext.InsuranceTypes.Any(e => e.Id == id);
        }
        /// <summary>
        /// zjistí jestli uz produkt existuje
        /// </summary>
        /// <param name="title">název</param>
        /// <returns>logickou hodnotu</returns>
        private bool InsuranceTypeExists(string title)
        {
            return _dbContext.InsuranceTypes.Any(e => e.Title == title);
        }

    }
}
