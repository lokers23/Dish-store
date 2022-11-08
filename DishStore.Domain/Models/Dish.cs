using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DishStore.Models
{
    public partial class Dish
    {
        public Dish()
        {
            DishOrders = new HashSet<DishOrder>();
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Укажите название")]
        [MaxLength(50, ErrorMessage = "Имя должно иметь длину меньше 50 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
        public string Name { get; set; } = null!;
        
        [Required(ErrorMessage = "Укажите материал")]
        [MaxLength(50, ErrorMessage = "Имя должно иметь длину меньше 50 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
        public string Material { get; set; } = null!;
        
        [Required(ErrorMessage = "Укажите стоимость")]
        public decimal Cost { get; set; }
        public string? ImagePath { get; set; }
        public int? CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual ICollection<DishOrder> DishOrders { get; set; }
    }
}
