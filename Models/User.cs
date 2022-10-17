using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DishStore.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public int? Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
