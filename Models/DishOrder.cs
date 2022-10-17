using System;
using System.Collections.Generic;

namespace DishStore.Models
{
    public partial class DishOrder
    {
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }

        public virtual Dish Dish { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
