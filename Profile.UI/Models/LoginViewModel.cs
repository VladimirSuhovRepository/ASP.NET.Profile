using System.ComponentModel.DataAnnotations;

namespace Profile.UI.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        [RegularExpression(@"^\s*[.\w@\-_]*\s*$")]
        [Display(Name = "Имя пользователя")]
        public string Login { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 6)]
        [RegularExpression(@"^[\w]*$")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}