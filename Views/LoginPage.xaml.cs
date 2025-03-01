namespace CivicFix.Views;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System;
using System.Collections.Generic;
using Twilio.Rest.Verify.V2.Service;

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
        if (txtMoblileNo.Equals("GarbX92MTA") && adminPassword.Equals("G@rbS3c92!")) 
        {
            await Shell.Current.GoToAsync(nameof(GAdminDashboard));
        }
         else if (txtMoblileNo.Equals("RoadZ76BQC") && adminPassword.Equals("R0adS3c76!"))
        {
            await Shell.Current.GoToAsync(nameof(RAdminDashboard));
        }

        pnlMobile.IsVisible = false;
        pnlMobileVerification.IsVisible = true;

    }

    
}