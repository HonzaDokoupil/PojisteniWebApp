using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Classes;
using PojisteniWebApp.Data;
using PojisteniWebApp.Models;


namespace PojisteniWebApp.Controllers
{
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Insured}")]
    public class InsuranceEventsController(ApplicationDbContext dbContext) : Controller
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        // GET: InsuranceEvents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _dbContext.InsuranceEvents
                .Include(i => i.IndividualContract)
                .Include(i => i.IndividualContract.InsuredPerson);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: InsuranceEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insuranceEvent = await _dbContext.InsuranceEvents
                .Include(i => i.IndividualContract)
                .Include(i => i.IndividualContract.InsuredPerson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceEvent == null)
            {
                return NotFound();
            }        
            return View(insuranceEvent);
        }

        // GET: InsuranceEvents/Create
        public IActionResult Create(int? individualContractId)
        {
            if (individualContractId == null)
            {
                return NotFound();
            }
            ViewData["IndividualContract"] = GetContractById(individualContractId);
            return View();
        }

        // POST: InsuranceEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,EventDate,IndividualContractId,DamageValue")] InsuranceEvent insuranceEvent)
        {
            // zkotrolujeme jestli hodnota škod nepřesáhla sluvní částku
            var contractId = insuranceEvent.IndividualContractId;
            if(insuranceEvent.DamageValue > GetContractById(contractId).Value)
            {
                TempData["error"] = "Chyba při vytváření události";
                ModelState.AddModelError("DamageValue", "Pokud výše škody přesahuje smluvní částku kontaktujte prosím svého pojišťovacího agenta");
                ViewData["IndividualContract"] = GetContractById(contractId);
                return View(insuranceEvent);
            }
            // zkontrolujeme jestli událost spadá do období na které máme smlouvu
            if(insuranceEvent.EventDate < GetContractById(contractId).FromDate ||
                insuranceEvent.EventDate > GetContractById(contractId).ToDate)
            {
                TempData["error"] = "Chyba při vytváření události";
                ModelState.AddModelError("EventDate", "Datum pojistné události musí spadat do období na které je sjednaná smlouva");
                ViewData["IndividualContract"] = GetContractById(contractId);
                return View(insuranceEvent);
            }

            if (ModelState.IsValid)
            {
                _dbContext.Add(insuranceEvent);
                await _dbContext.SaveChangesAsync();
                TempData["success"] = "Pojistná událost byla úspěšne vytvořena";
                return Redirect(Url.Action("Details", "IndividualContracts") + "/" + contractId);
            }
            TempData["error"] = "Chyba při vytváření události";
            ViewData["IndividualContract"] = GetContractById(contractId);
            return View(insuranceEvent);
        }
        // GET: InsuranceEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceEvent = await _dbContext.InsuranceEvents.FindAsync(id);
            if (insuranceEvent == null)
            {
                return NotFound();
            }
            ViewData["IndividualContract"] = _dbContext.IndividualContracts.Find(insuranceEvent.IndividualContractId);
            return View(insuranceEvent);
        }

        // POST: InsuranceEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,EventDate,IndividualContractId,DamageValue")] InsuranceEvent insuranceEvent)
        {
            var contractId = insuranceEvent.IndividualContractId;
            if (id != insuranceEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(insuranceEvent);
                    await _dbContext.SaveChangesAsync();
                    TempData["success"] = "Pojistná událost byla úspěšne upravena";
                }
                catch (DbUpdateConcurrencyException)
                {

                    return NotFound();
                    
                }
                return Redirect(Url.Action("Details", "IndividualContracts") + "/" + contractId);
            }
            TempData["error"] = "Chyba upravení události";
            ViewData["IndividualContract"] = GetContractById(contractId);
            return View(insuranceEvent);
        }

        // GET: InsuranceEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceEvent = await _dbContext.InsuranceEvents.Include(i => i.IndividualContract.InsuredPerson)
                .Include(i => i.IndividualContract)
                .Include(i => i.IndividualContract.InsuredPerson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceEvent == null)
            {
                return NotFound();
            }
            return View(insuranceEvent);
        }

        // POST: InsuranceEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuranceEvent = await _dbContext.InsuranceEvents.FindAsync(id);
            if (insuranceEvent != null)
            {
                _dbContext.InsuranceEvents.Remove(insuranceEvent);
            }
            else
            {
                return NotFound();
            }

            await _dbContext.SaveChangesAsync();
            TempData["success"] = "Pojistná událost byla úspěšne smazána";
            return Redirect(Url.Action("Details", "IndividualContracts") + "/" + insuranceEvent.IndividualContractId);
        }
        /// <summary>
        /// zjisti jestli smluva existuje
        /// </summary>
        /// <param name="id">číslo id</param>
        /// <returns>logická hodnota</returns>
        private bool InsuranceEventExists(int id)
        {
            return _dbContext.InsuranceEvents.Any(e => e.Id == id);
        }
        
        /// <summary>
        /// najde smlouvu podle její id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>smlouvu</returns>
        private IndividualContract GetContractById(int? id)
        {
            return  _dbContext.IndividualContracts.Find(id);
        }
    }
}
