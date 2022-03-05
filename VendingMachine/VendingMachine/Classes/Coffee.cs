using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Classes
{
    class Coffee : Beverage
    {
        public Coffee(string name, double price, int waterTemp = 95) : base(name, price, waterTemp)
        {
            SetIcon(5, 30);

            BeverageIngredients = new IngredientsEnum[] { IngredientsEnum.Sugar, IngredientsEnum.Coffee_Beans, IngredientsEnum.Milk };
        }
        public override bool Equals(object obj)
        {
            if (obj is Coffee)
            {
                Coffee c1 = (Coffee)obj;

                if (this.Name == c1.Name)
                { return true; }

            }

            return false;
        }
    }
}
