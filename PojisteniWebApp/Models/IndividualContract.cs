using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PojisteniWebApp.Models
{
    public class IndividualContract
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Produkt")]
        public int InsuranceTypeId { get; set; }
        [ForeignKey("InsuranceTypeId")]
        [ValidateNever]
        public InsuranceType? InsuranceType { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Částka")]

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Value { get; set; }                       
        [Required(ErrorMessage = "Toto pole je povinné")]    
        [DisplayName("Předmět pojištění")]                      
        [MaxLength(50)]
        public string? SubjectOfInsurance { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Platnost od")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Platnost do")]
        public DateTime ToDate { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Id Pojištěnce")]
        public int InsuredPersonId { get; set; }
        [ForeignKey("InsuredPersonId")]
        [DisplayName("Pojištěnec")]
        [ValidateNever]
        public InsuredPerson? InsuredPerson { get; set; }
    }
}
