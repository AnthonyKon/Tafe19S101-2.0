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
    public sealed partial class PersonalInfo : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public PersonalInfo()
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
            conn.CreateTable<PersonalInfoC>();
            var query1 = conn.Table<PersonalInfoC>();
            PersonalInfoView.ItemsSource = query1.ToList();
        }



        //ADD INFO
        private async void AddInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_FirstName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                if (_LastName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                if (_DOB.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                if (_Gender.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                if (_EmailAddress.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                if (_MobilePhone.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No value entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    //conn.CreateTable<PersonalInfo>();
                    conn.Insert(new PersonalInfoC
                    {
                        FirstName = _FirstName.Text.ToString(),
                        LastName = _LastName.Text.ToString(),
                        DOB = _DOB.Text.ToString(),
                        Gender = _Gender.Text.ToString(),
                        EmailAddress = _EmailAddress.Text.ToString(),
                        MobilePhone = _MobilePhone.Text.ToString()
                    });
                    // Creating table
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Value or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Similar last name already exists, Try a different last name", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }
        //DO THE DELETE LIKE AN ABSOLUTE CHAMPION
        private async void DeleteInfoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((PersonalInfoC)PersonalInfoView.SelectedItem).PersonalID.ToString();
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<PersonalInfoC>();
                    var query1 = conn.Table<PersonalInfoC>();
                    var query3 = conn.Query<PersonalInfoC>("DELETE FROM PersonalInfoC WHERE PersonalID ='" + AccSelection + "'");
                    PersonalInfoView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void EditInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((PersonalInfoC)PersonalInfoView.SelectedItem).PersonalID.ToString();
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {


                    var query1 = conn.Table<PersonalInfoC>();
                    var query3 = conn.Query<PersonalInfoC>("UPDATE PersonalInfoC SET FirstName = '" + _FirstName.Text + "', LastName = '" + _LastName.Text + "', DOB = '" + _DOB.Text + "', Gender = '" + _Gender.Text + "', EmailAddress = '" + _EmailAddress.Text + "', MobilePhone = '" + _MobilePhone.Text + "' WHERE PersonalID ='" + AccSelection + "'");
                    PersonalInfoView.ItemsSource = query1.ToList();
                }
            }

            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }
    }
}

