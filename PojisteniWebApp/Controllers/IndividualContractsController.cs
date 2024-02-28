using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Classes;
using PojisteniWebApp.Data;
using PojisteniWebApp.Models;


namespace PojisteniWebApp.Controllers
{
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Insured}")]
    public class IndividualContractsController(ApplicationDbContext dbContext) : Controller
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        // GET: IndividualContracts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _dbContext.IndividualContracts.Include(i => i.InsuredPerson).Include(i => i.InsuranceType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IndividualContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualContract = await _dbContext.IndividualContracts
                .Include(i => i.InsuredPerson)
                .Include(i => i.InsuranceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (individualContract == null)
            {
                return NotFound();
            }	
			individualContract.Events = _dbContext.InsuranceEvents.Where(m => m.IndividualContractId == id);
            return View(individualContract);
        }

        // GET: IndividualContracts/Create
        public IActionResult Create(int? insuredPersonId)
        {
            if (insuredPersonId == null)
            {
                return NotFound();
            }
            ViewData["InsuranceTypeId"] = new SelectList(_dbContext.InsuranceTypes, "Id", "Title");
            ViewData["InsuredPerson"] = _dbContext.InsuredPersons.Find(insuredPersonId);
            return View();
        }

        // POST: IndividualContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InsuranceTypeId,Value,SubjectOfInsurance,FromDate,ToDate,InsuredPersonId")] IndividualContract individualContract)
        {
            // Zjistíme jetli uz smlouva na předmět existuje a kdyz ano vyhodime hlašku 
            if (IndividualContractExists(individualContract.SubjectOfInsurance, individualContract.InsuredPersonId))
            {
                ViewData["InsuranceTypeId"] = new SelectList(_dbContext.InsuranceTypes, "Id", "Title", individualContract.InsuranceTypeId);
                ViewData["InsuredPerson"] = _dbContext.InsuredPersons.Find(individualContract.InsuredPersonId);
                TempData["error"] = "Tento předmět již máte pojištěný";
                return View(individualContract);
            }

            //zkotrolujeme jestli je koncove datum skutecne az po pocatecnim datu
            var toDate = individualContract.ToDate;
            var fromDate = individualContract.FromDate;
            if(fromDate == toDate || fromDate > toDate)
            {
                TempData["error"] = "Chyba při vytváření smlouvy";
                ModelState.AddModelError("ToDate", "Datum do kdy smlouva platí musí nasledovat za datem od kterého platí");
                ViewData["InsuranceTypeId"] = new SelectList(_dbContext.InsuranceTypes, "Id", "Title", individualContract.InsuranceTypeId);
                ViewData["InsuredPerson"] = _dbContext.InsuredPersons.Find(individualContract.InsuredPersonId);
                return View(individualContract);
            }

            if (ModelState.IsValid)
            {
                _dbContext.Add(individualContract);
                await _dbContext.SaveChangesAsync();
                TempData["success"] = "Smlouva byla úspěšne vytvořena";
                return Redirect(Url.Action("Details", "InsuredPersons") + "/" + individualContract.InsuredPersonId);
            }
            ViewData["InsuranceTypeId"] = new SelectList(_dbContext.InsuranceTypes, "Id", "Title", individualContract.InsuranceTypeId); 
            ViewData["InsuredPerson"] = _dbContext.InsuredPersons.Find(individualContract.InsuredPersonId);
            TempData["error"] = "Chyba při vytváření smlouvy";
            return View(individualContract);
        }

        // GET: IndividualContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualContract = await _dbContext.IndividualContracts.FindAsync(id);
            if (individualContract == null)
            {
                return NotFound();
            }
            ViewData["InsuranceType"] = _dbContext.InsuranceTypes.Find(individualContract.InsuranceTypeId);
            ViewData["InsuranceTypeId"] = new SelectList(_dbContext.InsuranceTypes, "Id", "Title");
            ViewData["InsuredPerson"] = _dbContext.InsuredPersons.Find(individualContract.InsuredPersonId);
            return View(individualContract);
        }

        // POST: IndividualContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InsuranceTypeId,Value,SubjectOfInsurance,FromDate,ToDate,InsuredPersonId")] IndividualContract individualContract)
        {
            if (id != individualContract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(individualContract);
                    await _dbContext.SaveChangesAsync();
                    TempData["success"] = "Smlouva byla úspěšne upravena";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndividualContractExists(individualContract.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect(Url.Action("Details", "InsuredPersons") + "/" + individualContract.InsuredPersonId);
            }
            ViewData["InsuranceType"] = _dbContext.InsuranceTypes.Find(individualContract.InsuranceTypeId);
            ViewData["InsuranceTypeId"] = new SelectList(_dbContext.InsuranceTypes, "Id", "Title", individualContract.InsuranceTypeId);
            ViewData["InsuredPerson"] = _dbContext.InsuredPersons.Find(individualContract.InsuredPersonId);
            TempData["error"] = "Chyba upravení smlouvy";
            return View(individualContract);
        }

        // GET: IndividualContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualContract = await _dbContext.IndividualContracts
                .Include(i => i.InsuranceType)
                .Include(i => i.InsuredPerson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (individualContract == null)
            {
                return NotFound();
            }
            individualContract.Events = _dbContext.InsuranceEvents.Where(m => m.IndividualContractId == id);
            return View(individualContract);
        }

        // POST: IndividualContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var individualContract = await _dbContext.IndividualContracts.FindAsync(id);
            if (individualContract != null)
            {
                _dbContext.IndividualContracts.Remove(individualContract);
            }
            else
            {
                return NotFound();
            }
            await _dbContext.SaveChangesAsync();
            TempData["success"] = "Smlouva byla úspěšne smazána";
            return Redirect(Url.Action("Details", "InsuredPersons") + "/" + individualContract.InsuredPersonId);
        }
        /// <summary>
        /// zjistí jestli smlouva existuje
        /// </summary>
        /// <param name="id">číslo id</param>
        /// <returns>logická hodnota</returns>
        private bool IndividualContractExists(int id)
        {
            return _dbContext.IndividualContracts.Any(e => e.Id == id);
        }
        /// <summary>
        /// zjistí jestli smlouva existuje
        /// </summary>
        /// <param name="subjectOdInsurance">předmět pojištění</param>
        /// <returns>logická hodnota</returns>
        private bool IndividualContractExists(string subjectOdInsurance, int id)
        {
            return _dbContext.IndividualContracts.Any(e => e.SubjectOfInsurance == subjectOdInsurance && e.InsuredPersonId == id);
        }
    }
}
