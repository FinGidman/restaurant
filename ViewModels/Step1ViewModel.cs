using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class Step1ViewModel
    {
        [Required]
        [Display(Name = "Выберите дату")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Количество гостей")]
        public int Persons { get; set; }

        public string ReturnUrl { get; set; }
    }
}
