using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VendingMachine.Classes;
using Windows.UI.Popups;
using System.Text;
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VendingMachine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Beverage BeverageChosen { get; set; }
        private MachineManager machineManager;
        private bool AddedSugar;
        private double _inputChange;
        private const char CURRENCY = '\u20AA';
        private Button[] _changeArrBTN;
        public MainPage()
        {

            this.InitializeComponent();
            machineManager = new MachineManager();


            #region setting money change buttons
            _changeArrBTN = new Button[] { BTN_InsertChange1, BTN_InsertChange2, BTN_InsertChange3, BTN_InsertChange4, BTN_InsertChange5, BTN_InsertChange6 };
            for (int i = 0; i < _changeArrBTN.Length; i++)
            {
                _changeArrBTN[i].FontSize = 30;
                _changeArrBTN[i].Width = 100;
                _changeArrBTN[i].Height = 45;
                _changeArrBTN[i].Margin = new Thickness(7);
                _changeArrBTN[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                _changeArrBTN[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                _changeArrBTN[i].VerticalAlignment = VerticalAlignment.Stretch;
                _changeArrBTN[i].VerticalContentAlignment = VerticalAlignment.Stretch;

                _changeArrBTN[i].Click += BTN_InsertChange_Click;
            }
            #endregion

            DisableXamlObj(BTN_PrepareDrink, BTN_FinishPurchase, CB_AddedSugar);

            BTN_FinishPurchase.Visibility = Visibility.Collapsed;
            TB_ChangeLeft.Visibility = Visibility.Collapsed;
            ClearScreen();
        }
       
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Beverage)
            {
                BeverageChosen = (Beverage)e.ClickedItem;

                //check if theres enough sugar for extra spoon
                if (machineManager.Vending.CheckIngredientStockCounter(IngredientsEnum.Sugar) > 1)
                {
                    EnableXamlObj(CB_AddedSugar);
                }

                EnableXamlObj(BTN_PrepareDrink);
            }
            else
            {
                throw new Exception("item clicked is not bev");
            }
        }

        private void BTN_ClearAmount_Click(object sender, RoutedEventArgs e)
        {
            _inputChange = 0;
            TB_InsertedAmount.Text = _inputChange.ToString();

            DisableXamlObj(BTN_ClearAmount, BTN_PrepareDrink);

        }

        private async void BTN_PrepareDrink_Click(object sender, RoutedEventArgs e)
        {
            double num = 0;
            double.TryParse(TB_InsertedAmount.Text, out num);

            if (BeverageChosen == null)
            {
                await new MessageDialog("Please choose beverage first").ShowAsync();
            }

            else if (BeverageChosen.Price > _inputChange)
            {
                await new MessageDialog($@"Chosen beverage costs {BeverageChosen.Price}{CURRENCY} " +
                    $"You need to insert another {BeverageChosen.Price - _inputChange}{CURRENCY}").ShowAsync();
            }

            else if (BeverageChosen != null && _inputChange >= BeverageChosen.Price)
            {
                double tempNum = _inputChange;

                TB_Preperation.Text = machineManager.OrderDrink(BeverageChosen, AddedSugar, ref _inputChange);

                bool transactionSuccessful = (tempNum != _inputChange);
                BTN_FinishPurchase.Content = transactionSuccessful ? "Take Beverage and Change" : "Take Change Back";
                if (!transactionSuccessful)
                {
                    await new MessageDialog($"Chosen beverage is not available").ShowAsync();
                }

                TB_ChangeLeft.Text = $"Your Change: {_inputChange}{CURRENCY}";
                TB_ChangeLeft.Visibility = Visibility.Visible;
                BTN_FinishPurchase.Visibility = Visibility.Visible;


                EnableXamlObj(BTN_FinishPurchase);
                DisableXamlObj(LV_Beverages, BTN_InsertChange1, BTN_InsertChange2, BTN_InsertChange3, BTN_InsertChange4, BTN_InsertChange5, BTN_InsertChange6);

                ClearScreen();
            }
        }

        private void ClearScreen()
        {
            _inputChange = 0;
            TB_InsertedAmount.Text = _inputChange.ToString();

            DisableXamlObj(BTN_ClearAmount, CB_AddedSugar, BTN_PrepareDrink);


            CB_AddedSugar.IsChecked = false;
            AddedSugar = (bool)CB_AddedSugar.IsChecked;


            LV_Beverages.SelectedItem = null;

        }

        private void CB_AddedSugar_Check(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox;
            if (e.OriginalSource is CheckBox)
            {
                checkBox = (CheckBox)e.OriginalSource;
                AddedSugar = (bool)checkBox.IsChecked;
            }
        }

        private void BTN_InsertChange_Click(object sender, RoutedEventArgs e)
        {
            double num;

            Button changeInsertedBTN;

            if (e.OriginalSource is Button)
            {
               
                    changeInsertedBTN = (Button)e.OriginalSource;
                    double.TryParse(changeInsertedBTN.Content.ToString(), out num);
                    _inputChange += num;
                    TB_InsertedAmount.Text = _inputChange.ToString();

                    EnableXamlObj(BTN_ClearAmount, BTN_PrepareDrink);
              
            }
            else
            {
                throw new Exception("selected object is not of type button");
            }


        }

        private void BTN_FinishPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button)
            {
                TB_Preperation.Text = "";
                TB_ChangeLeft.Text = "";
                TB_ChangeLeft.Visibility = Visibility.Collapsed;
                BTN_FinishPurchase.Visibility = Visibility.Collapsed;

                EnableXamlObj(LV_Beverages, BTN_InsertChange1, BTN_InsertChange2, BTN_InsertChange3, BTN_InsertChange4, BTN_InsertChange5, BTN_InsertChange6);

            }

            else
            {
                throw new Exception("selected object is not of type button");
            }
        }

        /// <summary>
        /// Disables accessability for items of type Button, CheckBox,ListView
        /// </summary>
        private void DisableXamlObj(params Object[] obj)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] is Button)
                {
                    Button btn = (Button)obj[i];
                    btn.IsEnabled = false;
                }
                else if (obj[i] is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)obj[i];
                    checkBox.IsEnabled = false;
                }
                else if (obj[i] is ListView)
                {
                    ListView listView = (ListView)obj[i];
                    listView.IsEnabled = false;
                }
                else
                {
                    throw new Exception($"object of type {obj[i].GetType()} does not fit to this method (as it only takes types: Button, CheckBox or ListView)");
                }
            }
        }

        /// <summary>
        /// Enables accessability for items of type Button, CheckBox,ListView
        /// </summary>
        private void EnableXamlObj(params Object[] obj)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] is Button)
                {
                    Button btn = (Button)obj[i];
                    btn.IsEnabled = true;
                    //change look to light mode
                }
                else if (obj[i] is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)obj[i];
                    checkBox.IsEnabled = true;
                }
                else if (obj[i] is ListView)
                {
                    ListView listView = (ListView)obj[i];
                    listView.IsEnabled = true;
                }
                else
                {
                    throw new Exception($"object of type {obj[i].GetType()} does not fit to this method (as it only takes types: Button, CheckBox or ListView)");
                }
            }
        }

        private void BTN_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
