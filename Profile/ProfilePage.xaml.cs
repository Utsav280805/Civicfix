using Microsoft.Maui.Controls;
namespace MauiApp3;

    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            // You can load user profile data here if available, e.g., from a database or API
            UsernameLabel.Text = "John Doe";  // Replace with dynamic data
            MobileLabel.Text = "+91 123 456 7890";  // Replace with dynamic data
        }

        // Event handler for editing the mobile number
        private async void EditMobile_Clicked(object sender, EventArgs e)
        {
            // You can show an input prompt or navigate to another page for editing
            string newMobile = await DisplayPromptAsync("Edit Mobile Number", "Enter your new mobile number:", initialValue: MobileLabel.Text);
            if (!string.IsNullOrEmpty(newMobile))
            {
                MobileLabel.Text = newMobile;
            }
        }


        // Event handler for viewing user's complaints
        private void YourComplaints_Clicked(object sender, EventArgs e)
        {
            // Navigate to the page that shows user's complaints (e.g., ComplaintListPage)
            // Assuming you have a ComplaintsPage to show complaints
            Navigation.PushAsync(new YourComplaint());  // Replace with your actual page name
        }

        // Event handler for sign out button
        private async void SignOut_Clicked(object sender, EventArgs e)
        {
            // Show confirmation dialog before signing out
            var answer = await DisplayAlert("Sign Out", "Are you sure you want to sign out?", "Yes", "No");
            if (answer)
            {
                // Handle sign-out logic (clear session, token, etc.)
                // Redirect to login page or home page
                await Navigation.PushAsync(new MainPage());  // Replace with your actual login page
            }
        }
    }
