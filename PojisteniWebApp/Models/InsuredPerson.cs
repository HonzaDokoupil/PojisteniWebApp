using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PojisteniWebApp.Models
{
    public enum Status
    {
        [Display(Name = "Pojištěnec")]
        Insured,
        [Display(Name = "Pojistitel")]
        Policyholder
    }
        public class InsuredPerson
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [StringLength(50)]
        [DisplayName("Jméno")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [StringLength(50)]
        [DisplayName("Příjmení")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [Phone(ErrorMessage = "Neplatné telefonní číslo")]
        [DisplayName("Telefon")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [StringLength(50)]
        [DisplayName("Ulice a číslo popisné")]
        public string? Adress { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [StringLength(50)]
        [DisplayName("Město")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Toto pole je povinné")]
        [DataType(DataType.PostalCode)]
        [MinLength(6, ErrorMessage = "Zadejte PSČ ve formátu XXX_XX")]
        [MaxLength(6)]
        [DisplayName("PSČ")]
        public string? PostalCode { get; set; }
        [Required(ErrorMessage = "Vyberte prosím jednu z možností")]
        [DisplayName("Status")]
        public Status Status { get; set; }
    }
}
