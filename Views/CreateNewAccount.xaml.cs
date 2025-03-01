using CivicFix.Services;
using System;
using System.Threading.Tasks;
namespace CivicFix.Views;

public partial class CreateNewAccount : ContentPage
{
    private readonly IOTPService _otpService;
    public CreateNewAccount()
    {
        InitializeComponent();
        _otpService = new TwilioOTPService();

    }

    private async void btncvarify_Clicked(object sender, EventArgs e)
    {
        string mobileNumber = MobileNumberEntry.Text;
        string username = UserName.Text;
        string password = Password.Text;
        string Apassword = APassword.Text;
        if (!string.IsNullOrWhiteSpace(mobileNumber) && mobileNumber.Length == 10)
        {
            if(string.IsNullOrWhiteSpace(username))
            {
                await DisplayAlert("Invalid", "Entered Username is Invalid please...! ", "Try Again");
                return;
            }
            if (password != Apassword && password.Length>=8)
            {
                await DisplayAlert("Error", " The Passwords are not Same !! Please ReEnter", "Ok");
                return;
            }

            bool success = await _otpService.GenerateAndSendOTPAsync(mobileNumber);


            if (success)
            {
                pnlMobile.IsVisible = false;
                pnlMobileVerification.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Error", " Failed to send OTP.", "Ok");
            }
        }
        else
        {
            await DisplayAlert("Error", "Please enter a valid phone number.", "OK");
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

        if (!string.IsNullOrWhiteSpace(mobileNumber) && !string.IsNullOrWhiteSpace(enteredOTP))
        {
            bool isVerified = _otpService.VerifyOTP(mobileNumber, enteredOTP);
            if (isVerified)
            {
                await Shell.Current.GoToAsync("UserDashbord");
            }
            else
            {
                await DisplayAlert("Error", "Please enter the Valid OTP.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Please enter the OTP.", "OK");
        }
    }

   
}