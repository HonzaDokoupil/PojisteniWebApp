using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Classes;
using PojisteniWebApp.Data;
using PojisteniWebApp.Models;

namespace PojisteniWebApp.Controllers
{
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Insured}")]
    public class InsuredPersonsController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        // GET: InsuredPersons
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.InsuredPersons.ToListAsync());
        }

        // GET: InsuredPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insuredPerson = await _dbContext.InsuredPersons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuredPerson == null)
            {
                return NotFound();
            }
            insuredPerson.Contracts = _dbContext.IndividualContracts.Where(m => m.InsuredPersonId == id).Include(i => i.InsuranceType);
            return View(insuredPerson);
        }

        // GET: InsuredPersons/Create
        [Authorize(Roles = $"{UserRoles.Admin}")]
        public IActionResult Create()
        {
			return View();
        }
        public IActionResult CreateOnRegister()
        {
            return View();
        }

        // POST: InsuredPersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Phone,Adress,City,PostalCode,Policyholder")] InsuredPerson insuredPerson)
        {
            if (InsuredPersonExists(insuredPerson.Email))
            {
                TempData["error"] = "Pojištěnec s tímto emailem už v databázi je";
                return View(insuredPerson);
            }
            if (ModelState.IsValid)
            {
                _dbContext.Add(insuredPerson);
                await _dbContext.SaveChangesAsync();
                // když vytvoříme noveho pojištěnce podívame se jestli ma zalozeny učet a jestli ano tak mu přiřadíme roli
                IdentityUser? identityUser = _userManager.Users.FirstOrDefault(user => user.Email == insuredPerson.Email);          
                if (identityUser != null)
                {
                    await _userManager.AddToRoleAsync(identityUser, UserRoles.Insured);
                }
                TempData["success"] = "Pojištěnec byl úspěšne vytvořen";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Chyba při vytváření pojištěnce";
            return View(insuredPerson);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOnRegister([Bind("Id,FirstName,LastName,Email,Phone,Adress,City,PostalCode,Policyholder")] InsuredPerson insuredPerson)
        {

            if (ModelState.IsValid)
            {
                _dbContext.Add(insuredPerson);
                await _dbContext.SaveChangesAsync();
                TempData["success"] = "Pojištěnec byl úspěšne vytvořen";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Chyba při vytváření pojištěnce";
            return View(insuredPerson);
        }
        // GET: InsuredPersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuredPerson = await _dbContext.InsuredPersons.FindAsync(id);
            if (insuredPerson == null)
            {
                return NotFound();
            }
            return View(insuredPerson);
        }

        // POST: InsuredPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,Adress,City,PostalCode,Policyholder")] InsuredPerson insuredPerson)
        {
            if (id != insuredPerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(insuredPerson);
                    await _dbContext.SaveChangesAsync();
                    TempData["success"] = "Pojištěnec byl úspěšne upraven";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuredPersonExists(insuredPerson.Id))
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
            TempData["error"] = "Chyba upravení pojištěnce";
            return View(insuredPerson);
        }

        // GET: InsuredPersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuredPerson = await _dbContext.InsuredPersons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuredPerson == null)
            {
                return NotFound();
            }
            insuredPerson.Contracts = _dbContext.IndividualContracts.Where(m => m.InsuredPersonId == id).Include(i => i.InsuranceType);
            return View(insuredPerson);
        }

        // POST: InsuredPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuredPerson = await _dbContext.InsuredPersons.FindAsync(id);
            if (insuredPerson != null)
            {
                _dbContext.InsuredPersons.Remove(insuredPerson);
                // pokud mažeme pojištence ktery ma zaroven zalozeny ucet s rolí pojištěnce, tuto roli mu odebereme a smažeme mu účet
                IdentityUser? identityUser = _userManager.Users.FirstOrDefault(user => user.Email == insuredPerson.Email);
                if (identityUser != null)
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, UserRoles.Insured);
                    await _userManager.DeleteAsync(identityUser);
                }
            }
            await _dbContext.SaveChangesAsync();
            TempData["success"] = "Pojištěnec byl úspěšne smazán";
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Zjistí jestli pojištěná osoba existuje 
        /// </summary>
        /// <param name="email">textový řetězec predstavující jeho email</param>
        /// <returns>logická hodnota</returns>
        private bool InsuredPersonExists(string email)
        {
            return _dbContext.InsuredPersons.Any(e => e.Email == email);
        }
        /// <summary>
        /// Zjistí jestli pojištěná osoba existuje 
        /// </summary>
        /// <param name="id">číslo id</param>
        /// <returns>logická hodnota</returns>
        private bool InsuredPersonExists(int id)
        {
            return _dbContext.InsuredPersons.Any(e => e.Id == id);
        }
    }
}
