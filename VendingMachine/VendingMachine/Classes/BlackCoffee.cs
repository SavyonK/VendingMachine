using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Classes
{
    class BlackCoffee : Coffee
    {
        public BlackCoffee(string name, double price, int waterTemp = 95) : base(name, price, waterTemp)
        {
            SetIcon(1, 30);

            BeverageIngredients = new IngredientsEnum[] { IngredientsEnum.Coffee_Beans };
        }
        public override bool Equals(object obj)
        {
            if (obj is BlackCoffee)
            {
                BlackCoffee c1 = (BlackCoffee)obj;

                if (this.Name == c1.Name)
                { return true; }

            }

            return false;
        }
    }
}
