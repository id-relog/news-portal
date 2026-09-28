using System.ComponentModel.DataAnnotations;

namespace WebAppUI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; } = null!;
    }
}