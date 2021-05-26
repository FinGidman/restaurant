using Restaurant.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class OrderDishViewModel
    {
        [Display(Name = "На вынос")]
        public string Type { get; set; }

        [Display(Name = "Ваши столы")]
        public IList<TableOrder> OrderedTables { get; set; }

        public int TableId { get; set; }

        public DateTime TableDate { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment {get; set;}
    }
}
