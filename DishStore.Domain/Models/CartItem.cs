namespace DishStore.Models
{
    public class CartItem
    {
        //public int DishId { get; set; }
        //public string DishName { get; set; }
        //public string DishMaterial { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
        //public decimal Cost { get; set; }
        //public int? CategoryId { get; set; }
        //public int ManufacturerId { get; set; }
        public decimal Total
        {
            get { return Quantity * Dish.Cost; }
        }
        public string? ImagePath { get; set; }

        public CartItem()
        {
        }

        public CartItem(Dish dish)
        {
            //DishId = dish.Id;
            //DishName = dish.Name;
            //DishMaterial = dish.Material;
            //Cost = dish.Cost;
            //ImagePath = dish.ImagePath;
            //CategoryId = dish.CategoryId;
            //ManufacturerId = dish.ManufacturerId;
            Dish = dish;
            Quantity = 1;

        }
    }
}
