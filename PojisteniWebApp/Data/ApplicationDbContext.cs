using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PojisteniWebApp.Models;

namespace PojisteniWebApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<PojisteniWebApp.Models.InsuranceType> InsuranceTypes { get; set; }
        public DbSet<PojisteniWebApp.Models.InsuredPerson> InsuredPersons { get; set; }
        public DbSet<PojisteniWebApp.Models.IndividualContract> IndividualContracts { get; set; }
        public DbSet<PojisteniWebApp.Models.InsuranceEvent> InsuranceEvents { get; set; }
        public DbSet<PojisteniWebApp.Models.Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InsuranceType>().HasData(
                new InsuranceType { Id = 1, Title = "Pojištění majetku", Description = "Pojištění majetku je souhrnné označení pro několik odvětví neživotního pojištění. Všechna jsou upravena v Hlavě III zákona o pojistné smlouvě „Soukromé pojištění věci a jiného majetku“. Při těchto pojištěních poskytuje pojišťovna pojistnou ochranu majetku v případě, že dojde k jeho zničení, poškození nebo odcizení." }
                );
            modelBuilder
                    .Entity<InsuredPerson>()
                    .Property(p => p.Status)
                    .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));
        }
    }
}
