using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace VendingMachine.Classes
{
    /// <summary>
    /// Vending Machine object holds & manages: beverage list, ingredient stock and money charged during runtime
    /// </summary>

    class VendingMachine
    {
        public ObservableCollection<Beverage> BeveragesList { get; private set; }
        public Dictionary<IngredientsEnum, int> CurrentStock { get; private set; }
        public double MoneyEarned { get; set; }


        public VendingMachine()
        {
            MoneyEarned = 0;

            #region initialize stock ingredients
            CurrentStock = new Dictionary<IngredientsEnum, int>();

            CurrentStock.Add(IngredientsEnum.Coffee_Beans, 0);
            CurrentStock.Add(IngredientsEnum.Milk, 0);
            CurrentStock.Add(IngredientsEnum.Sugar, 0);
            CurrentStock.Add(IngredientsEnum.Tea_Leaves, 0);
            CurrentStock.Add(IngredientsEnum.Cocoa_Powder, 0);

            #endregion

            BeveragesList = new ObservableCollection<Beverage>(); //beverages will be added by manager class using AddBeverage method
        }
        /// <summary>
        /// removes beverages containing unavailable ingredients
        /// </summary>
        public void RefreshBeverageListToCurrentStock()
        {
            for (int i = BeveragesList.Count - 1; i == 0; i--)
            {
                for (int j = 0; j < BeveragesList[i].BeverageIngredients.Length; j++)
                {
                    if (CurrentStock[BeveragesList[i].BeverageIngredients[j]] == 0)
                    {
                        BeveragesList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public bool CheckBeverageAvailability(Beverage beverage, bool extraSugar, double change)
        {
            bool isBeverageAvailable = false;
            if (BeveragesList.Contains(beverage) && beverage.Price <= change)
            {
                for (int i = 0; i < beverage.BeverageIngredients.Length; i++)
                {
                    if (CurrentStock[beverage.BeverageIngredients[i]] > 0)
                    {
                        isBeverageAvailable = true;
                        if (extraSugar && CurrentStock[IngredientsEnum.Sugar] == 1)
                        {
                            isBeverageAvailable = false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return isBeverageAvailable;

        }

        public int CheckIngredientStockCounter(IngredientsEnum ingredient)
        {
            if (CurrentStock.ContainsKey(ingredient))
            {
                return CurrentStock[ingredient];
            }
            return 0;

        }
        /// <summary>
        /// method for selecting a drink and preparing it, using the beverage "prepare" method
        /// removes ingredients used for preparing from stock
        /// </summary>
        /// <returns>will return empty string if beverage is unavailable</returns>
        public string OrderDrink(Beverage beverage, bool addMoreSugar, ref double change)
        {
            string str = "";

            if (CheckBeverageAvailability(beverage, addMoreSugar, change))
            {
                //removing beverage ingredients from vending machine stock
                for (int i = 0; i < beverage.BeverageIngredients.Length; i++)
                {
                    CurrentStock[beverage.BeverageIngredients[i]] = CurrentStock[beverage.BeverageIngredients[i]]- 1;
                }
                if (addMoreSugar)
                { CurrentStock[IngredientsEnum.Sugar] = CurrentStock[IngredientsEnum.Sugar]- 1; }

                change -= beverage.Price;
                MoneyEarned += beverage.Price;

                //prepare beverage
                str = BeveragesList[BeveragesList.IndexOf(beverage)].Prepare();
                if (addMoreSugar)
                {
                    if (str.Contains(IngredientsEnum.Sugar.ToString()))
                    {
                        str = str.Replace($"{IngredientsEnum.Sugar}", "Sugar (with extra by request)");
                    }
                    else
                    {
                        str = str.Replace(":", ": \nTeaspoon of sugar (by request)");
                    }

                }
            }

            return str; //will return empty string if unavailable
        }


        /// <summary>
        /// add beverage to general list of beverages
        /// </summary>
        public void AddBeverage(Beverage beverage)
        {
            bool beverageExists = false;

            for (int i = 0; i < BeveragesList.Count; i++)
            {
                if (BeveragesList[i].Equals(beverage))
                {
                    throw new Exception("Beverage is already in the list");
                }
            }
            if (!beverageExists)
            {
                BeveragesList.Add(beverage);
            }
        }


        /// <summary>
        /// remove beverage from general list of beverages
        /// </summary>      
        public void RemoveBeverage(Beverage beverage)
        {
            if (BeveragesList.Contains(beverage))
            {
                BeveragesList.Remove(beverage);
            }
            else
            {
                throw new Exception("Beverage wasnt in the list");
            }
        }


        public void RestockIngredient(IngredientsEnum ingredient, int stockAmount)
        {
            try
            {
                CurrentStock[ingredient] += stockAmount;

            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {

                throw new Exception("Ingredient must be inserted to stock list first!");
            }
        }



    }
}
