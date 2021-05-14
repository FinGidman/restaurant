using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant.Models
{
    public class Dish
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,Column(TypeName ="nvarchar(30)")]
        public string Name { get; set; }

        [Required,Column(TypeName = "nvarchar(30)")]
        public string Category { get; set; }

        [Column(TypeName="money")]
        public decimal Price { get; set; }

        [Required,Column(TypeName = "nvarchar(250)")]
        public string Description { get; set; }

        [Required, Column(TypeName = "nvarchar(250)")]
        public string Photo { get; set; }
    }
}
