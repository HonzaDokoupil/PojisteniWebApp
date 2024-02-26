using System.ComponentModel.DataAnnotations;

namespace PojisteniWebApp.Models.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Aktuální heslo je povinné")]
        [DataType(DataType.Password)]
        [Display(Name = "Aktuální heslo")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Nové heslo je povinné")]
        [StringLength(100, ErrorMessage = "Heslo musí být alespoň 8 znaků dlouhé", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Nové heslo")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Zadaná hesla se neshodují")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení nového hesla")]
        public string? ConfirmPassword { get; set; }
    }
}
