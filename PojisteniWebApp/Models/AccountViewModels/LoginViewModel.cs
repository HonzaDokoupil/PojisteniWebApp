using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PojisteniWebApp.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vyplňte emailovou adresu")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vyplňte heslo")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string? Password { get; set; }

        [Display(Name = "Pamatuj si mě")]
        public bool RememberMe { get; set; }
    }
}
