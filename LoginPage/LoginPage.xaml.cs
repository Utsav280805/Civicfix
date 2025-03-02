namespace MauiApp3;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System;
using System.Collections.Generic;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.Maui.Controls;

public partial class LoginPage : ContentPage
{
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
        string adminID = txtMoblileNo.Text;
        string adminPassword = Password.Text;
        if(string.IsNullOrEmpty(adminID) || string.IsNullOrEmpty(adminPassword))
        {
            await DisplayAlert("Error", "Please enter valid credentials", "OK");
            return;
        }
        await Shell.Current.GoToAsync(nameof(MainPage));



    }


}