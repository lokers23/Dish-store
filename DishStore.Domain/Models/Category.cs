using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DishStore.Models
{
    public partial class Category
    {
        public Category()
        {
            Dishes = new HashSet<Dish>();
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Укажите имя")]
        public string Name { get; set; } = null!;
        public string? ImagePath { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
