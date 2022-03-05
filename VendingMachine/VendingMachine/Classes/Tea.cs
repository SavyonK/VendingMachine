using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Classes
{
    class Tea : Beverage
    {

        public Tea(string name, double price, int waterTemp = 80) : base(name, price, waterTemp)
        {
            SetIcon(0, 30);

            BeverageIngredients = new IngredientsEnum[] { IngredientsEnum.Tea_Leaves };
        }

        public override bool Equals(object obj)
        {
            if (obj is Tea)
            {
                Tea t1 = (Tea)obj;

                if (this.Name == t1.Name)
                { return true; }
            }

            return false;
        }
    }
}
