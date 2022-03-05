using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Classes
{
    class ChaiTea : Tea
    {
        public ChaiTea(string name, double price, int waterTemp = 80) : base(name, price, waterTemp)
        {
            SetIcon(6, 30);

            BeverageIngredients = new IngredientsEnum[] {IngredientsEnum.Tea_Leaves,IngredientsEnum.Milk };
        }
        public override bool Equals(object obj)
        {
            if (obj is ChaiTea)
            {
                ChaiTea c1 = (ChaiTea)obj;

                if (this.Name == c1.Name)
                { return true; }

            }

            return false;
        }
    }
}
