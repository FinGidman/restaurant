using Microsoft.AspNetCore.Identity;
using Restaurant.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models.Data
{
    public class DishOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [ForeignKey("TableId")]
        public int TableId { get; set; }

        public bool Active { get; set; }

        [Column(TypeName ="nvarchar(10)")]
        public string OrderType { get; set; }

        public DateTime OrderTime { get; set; }

        [Required, Column(TypeName = "nvarchar(250)")]
        public string Comment { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string OrderListJson { get; set; }
        //
        public User User { get; set; }
        public Table Table { get; set; }
    }
}
