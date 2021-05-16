using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models.Data
{
    public class Table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //[Required]
        //public int Number { get; set; }

        [Required]
        public int Seats { get; set; }

        [Required, Column(TypeName = "nvarchar(250)")]
        public string SvgPath { get; set; }
    }
}
