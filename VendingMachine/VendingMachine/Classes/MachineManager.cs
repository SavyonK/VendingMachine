using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Classes
{
    /// <summary>
    /// creates Vending Machine object and manages it: order, adds & removes beverages, restocks ingredients
    /// </summary>

    class MachineManager
    {
        public VendingMachine Vending { get; set; }
     
        public MachineManager()
        {
          
            Vending = new VendingMachine();
            RestockVendingMachine();



            ChaiTea chai_tea = new ChaiTea("Chai Tea", 3);
            Vending.AddBeverage(chai_tea);
          
            Tea camomile_tea = new Tea("Camomile Tea", 2);
            Vending.AddBeverage(camomile_tea);

            BlackCoffee blackCoffee = new BlackCoffee("Black Coffee", 1.5);
            Vending.AddBeverage(blackCoffee);

            Coffee cappuccino = new Coffee("Cappuccino", 3.5);
            Vending.AddBeverage(cappuccino);
            
            Coffee espresso = new Coffee("Espresso", 3.5);
            Vending.AddBeverage(espresso);

            HotChocolate hot_chocolate = new HotChocolate("Hot Chocolate",4,78);
            Vending.AddBeverage(hot_chocolate);

           
        }
        public void RestockVendingMachine() 
        {
            Vending.RestockIngredient(IngredientsEnum.Cocoa_Powder, 300);
            Vending.RestockIngredient(IngredientsEnum.Tea_Leaves, 300);
            Vending.RestockIngredient(IngredientsEnum.Sugar, 300);
            Vending.RestockIngredient(IngredientsEnum.Milk, 300);
            Vending.RestockIngredient(IngredientsEnum.Coffee_Beans, 300);
        }
      

        public string OrderDrink(Beverage beverage, bool extraSugar,ref double change)
        {
            string endResult = "";

            if (Vending.CheckBeverageAvailability(beverage, extraSugar, change))
            {
                 endResult = Vending.OrderDrink(beverage, extraSugar, ref change);
            }
            //if (string.IsNullOrEmpty(endResult))
            //{
            //    endResult = "Chosen beverage is not available \nPlease choose a different one";

            //}
            else
            {
                endResult = "Chosen beverage is not available \nPlease choose a different one";
                Vending.RemoveBeverage(beverage);
            }

            Vending.RefreshBeverageListToCurrentStock();
            
            return endResult;

        }

    }
}
