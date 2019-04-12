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
using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingListPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ShoppingListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            Results();
        }
        public void Results()
        {
            conn.CreateTable<ShoppingList>();
            var query1 = conn.Table<ShoppingList>();
            ShoppingListView.ItemsSource = query1.ToList();
        }


        //ADD INFO
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShoppingItemID.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("This Auto Increments", "Caution..!");
                    await dialog.ShowAsync();
                }
                if (ShopName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Shop entered", "Caution..!");
                    await dialog.ShowAsync();
                }
                if (NameOfItem.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Item entered", "Caution..!");
                    await dialog.ShowAsync();
                }
                if (ShoppingDate.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Day/Date entered", "Caution..!");
                    await dialog.ShowAsync();
                }
                if (PriceQuoted.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Price entered", "Caution..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    //conn.CreateTable<ShoppingList>();
                    conn.Insert(new ShoppingList
                    {
                        ShoppingItemID = Int32.Parse(ShoppingItemID.Text),
                        ShopName = ShopName.Text.ToString(),
                        NameOfItem = NameOfItem.Text.ToString(),
                        ShoppingDate = ShoppingDate.Text.ToString(),
                        PriceQuoted = Int32.Parse(PriceQuoted.Text),

                    });
                    // Creating table
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Value or entered an invalid data", "Caution..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Repeated Data, Try Again", "Caution..!");
                    await dialog.ShowAsync();
                }
            }
        }
        //delete entry
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((ShoppingList)ShoppingListView.SelectedItem).ShoppingItemID.ToString();
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Caution..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<ShoppingList>();
                    var query1 = conn.Table<ShoppingList>();
                    var query3 = conn.Query<ShoppingList>("DELETE FROM ShoppingList WHERE ShoppingItemID ='" + AccSelection + "'");
                    ShoppingListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No item selected", "Caution..!");
                await dialog.ShowAsync();
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }


        //EDIT entry
        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((ShoppingList)ShoppingListView.SelectedItem).ShoppingItemID.ToString();
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("No Item Selected", "Caution..!");
                    await dialog.ShowAsync();
                }
                else
                {


                    var query1 = conn.Table<ShoppingList>();
                    var query3 = conn.Query<ShoppingList>("UPDATE ShoppingList SET ShoppingItemID = '" + ShoppingItemID.Text + "', ShopName = '" + ShopName.Text +
                                                            "', NameOfItem = '" + NameOfItem.Text + "', ShoppingDate = '" + ShoppingDate.Text + "', PriceQuoted = '" + PriceQuoted.Text + "' WHERE ShoppingItemID ='" + AccSelection + "'");

                    ShoppingListView.ItemsSource = query1.ToList();
                }
            }

            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Caution..!");
                await dialog.ShowAsync();
            }
        }
    }
}
