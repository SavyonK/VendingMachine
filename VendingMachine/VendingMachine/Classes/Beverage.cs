using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace VendingMachine.Classes
{
    /// <summary>
    ///  Base class for all Beverages
    /// </summary>
    public enum IngredientsEnum { Sugar, Coffee_Beans, Milk, Tea_Leaves, Cocoa_Powder }
    class Beverage
    {
        public  char Currency { get;private set; }
        private double _price;
        public double Price
        {
            get { return _price; }
            set
            {
                if (value > 0) { _price = value; }
                else { throw new Exception("Price cannot be 0 or negative"); }
            }
        }
        public string Name { get; private set; }
        public IngredientsEnum[] BeverageIngredients { get;protected set; }
        public int WaterTemp { get; private set; }
        public Image Icon { get; protected set; }
        public static List<Image> BeverageIcons { get; set; }
        public Beverage(string name, double price, int waterTemp)
        {
            Name = name;
            Price = price;
            Currency = '\u20AA';
            WaterTemp = waterTemp;
           
        }
        static Beverage()
        {
            
            BeverageIcons = new List<Image>();
            for (int i = 1; i <= 14; i++)
            {
                Image icon = new Image();
                icon.Source = new BitmapImage(new Uri($@"ms-appx://Project/Assets/BeverageIcons/{i}.png"));
                BeverageIcons.Add(icon);
            }

        }
        protected void SetIcon(int iconIndex,int size)
        {
            Icon = BeverageIcons[iconIndex];
            Icon.Height = size;
            Icon.Width = size;
        }
        public string Prepare()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Adding Ingredients: ");
            for (int i = 0; i < BeverageIngredients.Length; i++)
            {
                string a;
                
                if (BeverageIngredients[i].ToString().Contains('_'))
                { a = BeverageIngredients[i].ToString().Replace('_', ' '); }
                
                else { a = BeverageIngredients[i].ToString(); }
                
                sb.AppendLine(a+"...");
            }
            sb.AppendLine($"Adding {WaterTemp}\u00B0 Water");
            sb.AppendLine($"Stirring...");
            sb.AppendLine($"Your drink is now ready!");

            return sb.ToString();
        }

    }
}
