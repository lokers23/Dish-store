using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DishStore.Models
{
    public partial class Order
    {
        public Order()
        {
            DishOrders = new HashSet<DishOrder>();
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Укажите имя")]
        public string Name { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        [Required(ErrorMessage = "Укажите адрес")]
        public string Address { get; set; } = null!;
        
        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Пример: vova@gmail.com")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Пример: vova@gmail.com")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Укажите телефон")]
        [RegularExpression(@"^(\+)?((\d{2,3}) ?\d|\d)(([ -]?\d)|( ?(\d{2,3}) ?)){5,12}\d$", ErrorMessage = "Пример: +375299769895")]
        public string PhoneNumber { get; set; } = null!;
        public DateTime DateOrder { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<DishOrder> DishOrders { get; set; }
    }
}
