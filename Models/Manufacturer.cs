using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DishStore.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Dishes = new HashSet<Dish>();
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Укажите название")]
        public string Name { get; set; } = null!;
        
        [Required(ErrorMessage = "Укажите страну")]
        public string Country { get; set; } = null!;

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
