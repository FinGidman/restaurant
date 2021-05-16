using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class Step3ViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Номер столика")]
        public int TableId { get; set; }

        [Display(Name = "Количество гостей")]
        public int Persons { get; set; }

        [Required]
        [Display(Name ="Часы")]
        public int Hours { get; set; }

        [Required]
        [Display(Name = "Минуты")]
        public int Minutes { get; set; }

        [Required]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
    }
}
