using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PojisteniWebApp.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Počet produktů:")]
        public int InsuranceTypesCount { get; set; }
        [DisplayName("Počet pojištěnců:")]
        public int InsuredPersonsCount { get; set; }
        [DisplayName("Počet smluv:")]
        public int IndividualContractsCount { get; set; }
        [DisplayName("Počet pojistných událostí:")]
        public int InsuranceEventsCount { get; set; }
        [DisplayName("Počet registrovaných uživatelů:")]
        public int UsersCount { get; set; }
        [DisplayName("Celková pojištěná suma:")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal InsuredValueTotal {  get; set; }
        [DisplayName("Celková suma všech škod:")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DamageValueTotal { get; set; }
        [DisplayName("Počet pojištěncu bez smlouvy:")]
        public int PersonsWithNoContractsCount { get; set; }
        [DisplayName("Počet pojištěncu bez pojistných událostí:")]
        public int PersonsWithNoEventCount { get; set; }
        [DisplayName("Počet regitrovaných uživatelů kteří nejsou vedeni jako pojištěnci:")]
        public int UseresWithNoRole {  get; set; }
        [DisplayName("Počet pojištěneců bez založeného učtu:")]
        public int InsuredPersonsWithNoAccount { get; set; }
        [DisplayName("Pojištěnec s největší pojištěnou částkou:")]
        public string? PersonWithHighestInsuredValueTotal { get; set; }
        [DisplayName("Pojištěnec s nejmenší pojištěnou částkou:")]
        public string? PersonWithLowestInsuredValueTotal { get; set; }
        [DisplayName("Pojištěnec s největší škodou:")]
        public string? PersonWithHighestDamageValueTotal { get; set; }
        [DisplayName("Pojištěnec s nejmenší škodou:")]
        public string? PersonWithLowestDamageValueTotal { get; set; }
        [DisplayName("Nejvíce smlouvaný produkt:")]
        public string? MostContractedInsurenceType {  get; set; }
        [DisplayName("Nejméně smlouvaný produkt:")]
        public string? LeastContractedInsurenceType { get; set; }
        [DisplayName("Vytvořeno:")]
        public DateTime ReportDate { get; set; } = DateTime.Now;
    }
}
