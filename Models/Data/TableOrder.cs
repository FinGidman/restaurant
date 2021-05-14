using Microsoft.AspNetCore.Identity;
using Restaurant.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant.Models
{
    public class TableOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("TableId")]
        public int TableId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public DateTime OrderTime { get; set; }

        public bool Active { get; set; }

        public int Persons { get; set; }

        [Required, Column(TypeName ="nvarchar(250)")]
        public string Comment { get; set; }
        //
        public User User { get; set; }
        public Table Table { get; set; }
    }
}
