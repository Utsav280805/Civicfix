using MauiApp3.Database;
using MauiApp3.Models;
using MauiApp3.Service;
using MauiApp3.Service;
using MauiApp3;
using System;
using System.Threading.Tasks;

namespace MauiApp3;

public partial class CreateNewAccount : ContentPage
{
    private readonly IOTPService _otpService;
    private readonly DatabaseHelper _databaseHelper;

    public CreateNewAccount()
    {
        InitializeComponent();
        _otpService = new TwilioOTPService();
        _databaseHelper = new DatabaseHelper();
    }

    private async void btncvarify_Clicked(object sender, EventArgs e)
    {
        string mobileNumber = MobileNumberEntry.Text;
        string username = UserName.Text;
        string password = Password.Text;
        string confirmPassword = APassword.Text;

        if (string.IsNullOrWhiteSpace(mobileNumber) || mobileNumber.Length != 10)
        {
            await DisplayAlert("Error", "Please enter a valid 10-digit mobile number.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Error", "Username cannot be empty.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            await DisplayAlert("Error", "Password must be at least 8 characters long.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match. Please try again.", "OK");
            return;
        }

        // Check if the user already exists
        var existingUser = await _databaseHelper.GetUserByMobile(mobileNumber);
        if (existingUser != null)
        {
            await DisplayAlert("Error", "User already exists with this mobile number.", "OK");
            return;
        }

        // Send OTP
        bool success = await _otpService.GenerateAndSendOTPAsync(mobileNumber);
        if (success)
        {
            pnlMobile.IsVisible = false;
            pnlMobileVerification.IsVisible = true;
        }
        else
        {
            await DisplayAlert("Error", "Failed to send OTP.", "OK");
        }
    }

    private void resendOTP_Clicked(object sender, EventArgs e)
    {
        btncvarify_Clicked(sender, e);
    }

    private void changeNumber_Clicked(object sender, EventArgs e)
    {
        pnlMobileVerification.IsVisible = false;
        pnlMobile.IsVisible = true;
    }

    private async void btnVerifyCode_Clicked(object sender, EventArgs e)
    {
        string mobileNumber = MobileNumberEntry.Text;
        string enteredOTP = OTPCode.Text;
        string username = UserName.Text;
        string password = Password.Text;

        if (string.IsNullOrWhiteSpace(mobileNumber) || string.IsNullOrWhiteSpace(enteredOTP))
        {
            await DisplayAlert("Error", "Please enter the OTP.", "OK");
            return;
        }

        bool isVerified = _otpService.VerifyOTP(mobileNumber, enteredOTP);
        if (isVerified)
        {
            // Store the new user in the database
            User newUser = new User
            {
                MobileNumber = mobileNumber,
                Name = username,
                Password = password
            };

            int result = await _databaseHelper.AddUser(newUser);
            if (result > 0)
            {
                await DisplayAlert("Success", "Account Created Successfully!", "OK");
                await Shell.Current.GoToAsync("//MainPage");

            }
            else
            {
                await DisplayAlert("Error", "Failed to create an account.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Invalid OTP. Please try again.", "OK");
        }
    }
}
