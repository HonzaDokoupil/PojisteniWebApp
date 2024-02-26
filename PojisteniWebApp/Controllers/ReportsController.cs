using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Classes;
using PojisteniWebApp.Data;
using PojisteniWebApp.Models;


namespace PojisteniWebApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ReportsController(ApplicationDbContext dbContext, PDFGenerator generator, Info info) : Controller
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        /// <summary>
        /// Instance PDF generatoru predana DI konteinerem
        /// </summary>
        private readonly PDFGenerator _generator = generator;
        /// <summary>
        /// Info třída obsahující statistiku
        /// </summary>
        private readonly Info _info = info;

        // GET: Reports
        public IActionResult Index()
        {           
            _generator.GeneratePDF(_info);
            ViewBag.Info = _info;
            return View();
        }
        public async Task<IActionResult> List()
        {
            return View(await _dbContext.Reports.ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _dbContext.Reports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            _generator.GeneratePDF(report);
            return View(report);
        }
        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InsuranceTypesCount,InsuredPersonsCount,IndividualContractsCount,InsuranceEventsCount,UsersCount,InsuredValueTotal,DamageValueTotal,PersonsWithNoContractsCount,PersonsWithNoEventCount,UseresWithNoRole,InsuredPersonsWithNoAccount,PersonWithHighestInsuredValueTotal,PersonWithLowestInsuredValueTotal,PersonWithHighestDamageValueTotal,PersonWithLowestDamageValueTotal,MostContractedInsurenceType,LeastContractedInsurenceType,ReportDate")] Report report)   
        {
            if(ModelState.IsValid)
            {
                _dbContext.Add(report);
                await _dbContext.SaveChangesAsync();
                TempData["success"] = "Report byl úspěšně vytvořen";
                return RedirectToAction(nameof(Index));                
            }
            TempData["success"] = "Něco se pokazilo :(";
            return RedirectToAction(nameof(Index));

        }
        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _dbContext.Reports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _dbContext.Reports.FindAsync(id);
            if (report != null)
            {
                _dbContext.Reports.Remove(report);
            }
            await _dbContext.SaveChangesAsync();
            TempData["success"] = "Report byl úspěšně smazán";
            return RedirectToAction(nameof(List));
        }
        /// <summary>
        /// zjistí jestli report existuje
        /// </summary>
        /// <param name="id">číslo id</param>
        /// <returns>logická hodnota</returns>
        private bool ReportExists(int id)
        {
            return _dbContext.Reports.Any(e => e.Id == id);
        }
    }
}
