using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name="Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string PersonName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string PersonSurname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} должен иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }

    }
}
