using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Models;
using PojisteniWebApp.Data;

namespace PojisteniWebApp.Classes
{
    /// <summary>
    /// Toto je třída která ziská aktuální statistiku kterou pak lze uložit do databáze jako report
    /// </summary>
    /// <remarks>
    /// primary konstruktor
    /// </remarks>
    /// <param name="dbContext">databaze</param>
    public class Info(ApplicationDbContext dbContext)
    {
        /// <summary>
        /// hláška pro nedostatek dat v DB
        /// </summary>
        private const string missingData = "Pro tento údaj není v databázi dostatek dat";
        /// <summary>
        /// databazový kontext
        /// </summary>
        private readonly ApplicationDbContext _dbContext = dbContext;
        /// <summary>
        /// počet produktu
        /// </summary>
        public int InsuranceTypesCount { get => _dbContext.InsuranceTypes.Count(); }
        /// <summary>
        /// počet pojištěncu
        /// </summary>
        public int InsuredPersonsCount { get => _dbContext.InsuredPersons.Count(); }
        /// <summary>
        /// počet smluv
        /// </summary>
        public int IndividualContractsCount { get => _dbContext.IndividualContracts.Count(); }
        /// <summary>
        /// počet pojistných událostí
        /// </summary>
        public int InsuranceEventsCount { get => _dbContext.InsuranceEvents.Count(); }
        /// <summary>
        /// počet uživatelů
        /// </summary>
        public int UsersCount { get => _dbContext.Users.Count() - 1; } //minus admin
        /// <summary>
        /// celková pojištěná hodnota
        /// </summary>
        public decimal InsuredValueTotal { get => _dbContext.IndividualContracts.Sum(sum => sum.Value); }
        /// <summary>
        /// celková hodnota škod
        /// </summary>
        public decimal DamageValueTotal { get => _dbContext.InsuranceEvents.Sum(sum => sum.DamageValue); }
        /// <summary>
        /// počet pojištěncu bez smluv
        /// </summary>
        public int PersonsWithNoContractsCount
        {
            get => InsuredPersonsCount - _dbContext.IndividualContracts.GroupBy(contract => contract.InsuredPersonId).Count();
        }
        /// <summary>
        /// počet pojištěnců bez pojistných událostí
        /// </summary>
        public int PersonsWithNoEventCount { get => InsuredPersonsCount - _dbContext.InsuranceEvents.GroupBy(accident => accident.IndividualContract.InsuredPersonId).Count(); }
        /// <summary>
        /// uživatelé bez přiřazené role 
        /// </summary>
        public int UseresWithNoRole { get => UsersCount - (_dbContext.UserRoles.GroupBy(user => user.UserId).Count() - 1); }
        /// <summary>
        /// pojištěnci bez uživatelského účtu
        /// </summary>
        public int InsuredPersonsWithNoAccount { get => InsuredPersonsCount - (_dbContext.UserRoles.GroupBy(user => user.UserId).Count() - 1); } // minus admin
        /// <summary>
        /// Pojištenec s největší pojištěnou častkou
        /// </summary>
        public string PersonWithHighestInsuredValueTotal
        {
            get
            {
                int searchId = _dbContext.IndividualContracts.GroupBy(contract => contract.InsuredPersonId).Select(contract => new { Total = contract.Sum(x => x.Value), Id = contract.Key }).OrderByDescending(x => x.Total).Select(x => x.Id).FirstOrDefault();
                InsuredPerson insuredPerson = _dbContext.InsuredPersons.Find(searchId);
                return insuredPerson == null ? missingData : $"{insuredPerson.FirstName} {insuredPerson.LastName}";
            }
        }
        /// <summary>
        /// pojištěnec s nejnižší pojištěnou částkou
        /// </summary>
        public string PersonWithLowestInsuredValueTotal
        {
            get
            {
                InsuredPerson? insuredPerson = GetPersonWithNoContracts();
                if (insuredPerson == null)
                {
                    insuredPerson = _dbContext.InsuredPersons.Find(_dbContext.IndividualContracts.GroupBy(contract => contract.InsuredPersonId)
                         .Select(person => new { Total = person.Sum(x => x.Value), Id = person.Key }).OrderBy(x => x.Total)
                         .Select(x => x.Id).FirstOrDefault());
                    return insuredPerson == null ? missingData : $"{insuredPerson.FirstName} {insuredPerson.LastName}";
                }
                return $"{insuredPerson.FirstName} {insuredPerson.LastName}";
            }
        }
        /// <summary>
        /// pojištenec s největší častkou škod
        /// </summary>
        public string PersonWithHighestDamageValueTotal
        {
            get
            {
                InsuredPerson? insuredPerson = _dbContext.InsuredPersons.Find(_dbContext.InsuranceEvents.Include(i => i.IndividualContract)
                        .GroupBy(accident => accident.IndividualContract.InsuredPersonId)
                        .Select(contract => new { Total = contract.Sum(accident => accident.DamageValue), Id = contract.Key })
                        .OrderByDescending(x => x.Total).Select(x => x.Id).FirstOrDefault());
                return insuredPerson == null ? missingData : $"{insuredPerson.FirstName} {insuredPerson.LastName}";
            }
        }
        /// <summary>
        /// pojištěnec s nejmenší častkou škod
        /// </summary>
        public string PersonWithLowestDamageValueTotal
        {
            get
            {
                InsuredPerson? insuredPerson = GetPersonWithNoEvents();
                if (insuredPerson == null)
                {
                    insuredPerson = _dbContext.InsuredPersons.Find(_dbContext.InsuranceEvents
                            .GroupBy(accident => accident.IndividualContract.InsuredPersonId)
                            .Select(contract => new { Total = contract.Sum(accident => accident.DamageValue), Id = contract.Key })
                            .OrderBy(x => x.Total).Select(x => x.Id).FirstOrDefault());
                    return insuredPerson == null ? missingData : $"{insuredPerson.FirstName} {insuredPerson.LastName}";
                }
                return $"{insuredPerson.FirstName} {insuredPerson.LastName}";
            }
        }
        /// <summary>
        /// nejvíce smlouvaný produkt
        /// </summary>
        public string MostContractedInsurenceType
        {
            get
            {
                InsuranceType? insuranceType = _dbContext.InsuranceTypes.Find(_dbContext.IndividualContracts
                        .GroupBy(contract => contract.InsuranceTypeId)
                        .Select(type => new { Id = type.Key, Total = type.Count() }).OrderByDescending(contract => contract.Total)
                        .Select(x => x.Id).FirstOrDefault());
                return insuranceType == null ? missingData : $"{insuranceType.Title}";
            }
        }
        /// <summary>
        /// nejméne smlouvaný produkt
        /// </summary>
        public string LeastContractedInsurenceType
        {
            get
            {
                InsuranceType? insuranceType = GetInsuranceTypeWithNoContract();
                if (insuranceType == null)
                {
                    insuranceType = _dbContext.InsuranceTypes.Find(_dbContext.IndividualContracts
                        .GroupBy(contract => contract.InsuranceTypeId)
                        .Select(type => new { Id = type.Key, Total = type.Count() }).OrderBy(contract => contract.Total)
                        .Select(x => x.Id).FirstOrDefault());
                    return insuranceType == null ? missingData : $"{insuranceType.Title}";
                }
                return $"{insuranceType.Title}";
            }
        }
        /// <summary>
        /// datum reportu
        /// </summary>
        public DateTime ReportDate { get; private set; } = DateTime.Now;
        /// <summary>
        /// Získá produkt který od kterého není žadná smlouva
        /// </summary>
        /// <returns>produkt</returns>
        private InsuranceType? GetInsuranceTypeWithNoContract()
        {
            return _dbContext.InsuranceTypes.Where(type => !_dbContext.IndividualContracts.Select(x => x.InsuranceTypeId).ToArray().Contains(type.Id)).OrderBy(x => x.Id).FirstOrDefault();
        }
        /// <summary>
        /// Získá pojištěnce bez smluv
        /// </summary>
        /// <returns>pojištěnce</returns>
        private InsuredPerson? GetPersonWithNoContracts()
        {
            return _dbContext.InsuredPersons.Where(person => !_dbContext.IndividualContracts.Select(x => x.InsuredPersonId).ToArray().Contains(person.Id)).OrderBy(x => x.Id).FirstOrDefault();
        }
        /// <summary>
        /// Získá pojištěnce bez škod
        /// </summary>
        /// <returns>pojištěnce</returns>
        private InsuredPerson? GetPersonWithNoEvents()
        {
            return _dbContext.InsuredPersons.Where(person => !_dbContext.InsuranceEvents.Select(x => x.IndividualContract.InsuredPersonId).ToArray().Contains(person.Id)).OrderBy(x => x.Id).FirstOrDefault();
        }
    }
}
