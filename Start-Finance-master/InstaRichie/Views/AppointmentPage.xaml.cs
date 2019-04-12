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
using Windows.UI.Popups;
using StartFinance.Models;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AppointmentPage()
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
            // Creating table
            conn.CreateTable<Appointment>();
            var query = conn.Table<Appointment>();
            AppointmentList.ItemsSource = query.ToList();
        }

        // Displays the data when navigation between pages
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private void AddApp_Click(object sender, RoutedEventArgs e)
        {
            //string appDay = AppDate.Date.Day.ToString();
            //string appMonth = AppDate.Date.Month.ToString();
            //string appYear = AppDate.Date.Year.ToString();
            //string appStartMinute = AppStart.Time.Minutes.ToString();
            //string appStartHour = AppStart.Time.Hours.ToString();
            //string appEndMinute = AppEnd.Time.Minutes.ToString();
            //string appEndHour = AppEnd.Time.Hours.ToString();

            //string finalDate = appDay + "/" + appMonth + "/" + appYear;
            //string finalStartTime = appStartHour + ":" + appStartMinute;
            //string finalEndTime = appEndHour + ":" + appEndMinute;

            string finalDate = convertDateToString(AppDate.Date.Date);
            string finalStartTime = convertTimeToString(AppStart.Time);
            string finalEndTime = convertTimeToString(AppEnd.Time);

            conn.Insert(new Appointment()
            {
                EventName = AppName.Text,
                Location = AppLoc.Text,
                EventDate = finalDate,
                StartTime = finalStartTime,
                EndTime = finalEndTime
            });
            Results();

        }

        private async void DeleteApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AppSelection = ((Appointment)AppointmentList.SelectedItem).AppointmentID.ToString();
                if (AppSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Appointment>();
                    var query1 = conn.Table<Appointment>();
                    var query3 = conn.Query<Appointment>("DELETE FROM Appointment WHERE AppointmentID ='" + AppSelection + "'");
                    AppointmentList.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private async void UpdateApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AppSelection = ((Appointment)AppointmentList.SelectedItem).AppointmentID.ToString();
                if (AppSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    string finalDate = convertDateToString(AppDate.Date.Date);
                    string finalStartTime = convertTimeToString(AppStart.Time);
                    string finalEndTime = convertTimeToString(AppEnd.Time);

                    conn.CreateTable<Appointment>();
                    var query1 = conn.Table<Appointment>();
                    var query3 = conn.Query<Appointment>("UPDATE Appointment SET EventName = '" + AppName.Text + "', Location = '" + AppLoc.Text + "', EventDate = '" + finalDate + "', StartTime = '" + finalStartTime + "', EndTime = '" + finalEndTime + "' WHERE AppointmentID ='" + AppSelection + "'");
                    AppointmentList.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("No item selected", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private string convertDateToString(DateTime date)
        {
            string appDay = date.Date.Day.ToString();
            string appMonth = date.Date.Month.ToString();
            string appYear = date.Date.Year.ToString();
            string finalDate = appDay + "/" + appMonth + "/" + appYear;

            return finalDate;
        }

        private string convertTimeToString(TimeSpan time)
        {
            string appEndMinute = time.Minutes.ToString();
            string appEndHour = time.Hours.ToString();
            string finalTime = appEndHour + ":" + appEndMinute;

            return finalTime; 
        }
    }
}
