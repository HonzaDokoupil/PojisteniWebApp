using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PojisteniWebApp.Models
{
    public class InsuranceEvent
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Název")]
        [MaxLength(50)]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Popis")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Výše škody")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DamageValue { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Datum události")]
        public DateTime EventDate { get; set; }
        public int IndividualContractId { get; set; }
        [ForeignKey("IndividualContractId")]
        [ValidateNever]
        [DisplayName("Smlouva na")]
        public IndividualContract? IndividualContract { get; set; }

    }
}
