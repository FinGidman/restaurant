using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using restaurant.Models;

namespace Restaurant.Models.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Table> Tables { get; set; }
        public DbSet<TableOrder> TableOrders { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishOrder> DishOrders { get; set; }
    }
}
