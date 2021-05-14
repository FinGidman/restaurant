using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models.Data
{
    public class User : IdentityUser
    {
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
    }
}
