using System;
using MauiApp3.Database;
using MauiApp3;
using MauiApp3.Models;

namespace MauiApp3
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();

        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnCreatenewaccount_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(CreateNewAccount));
        }

        private async void btnlogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                string mobileNumber = txtMoblileNo.Text?.Trim();
                string password = Password.Text?.Trim();

                if (string.IsNullOrWhiteSpace(mobileNumber) || string.IsNullOrWhiteSpace(password))
                {
                    await DisplayAlert("Error", "Mobile Number and Password are required!", "OK");
                    return;
                }

                // Validate user in SQLite database
                var user = await _database.GetUser(mobileNumber, password);
                if (user == null)
                {
                    await DisplayAlert("Login Failed", "Invalid credentials! Please try again.", "OK");
                    return;
                }

                // Navigate to the main dashboard
                await Shell.Current.GoToAsync(nameof(MainPage));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
        }
    }
}
