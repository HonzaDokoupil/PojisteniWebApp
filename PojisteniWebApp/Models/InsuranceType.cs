using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PojisteniWebApp.Models
{
    public class InsuranceType
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [MaxLength(50)]
        [DisplayName("Název Pojištění")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DisplayName("Popis")]
        public string? Description { get; set; }
    }
}
