using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Classes
{
    class HotChocolate : Beverage
    {
        public HotChocolate(string name, double price, int waterTemp) : base(name, price, waterTemp)
        {
            SetIcon(3, 30);

            BeverageIngredients = new IngredientsEnum[] { IngredientsEnum.Cocoa_Powder, IngredientsEnum.Sugar, IngredientsEnum.Milk };
        }
        public override bool Equals(object obj)
        {
            if (obj is HotChocolate)
            {
                HotChocolate c1 = (HotChocolate)obj;

                if (this.Name == c1.Name)
                { return true; }

            }

            return false;
        }
    }
}
