namespace MauiApp3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(FirstPage), typeof(FirstPage));
            Routing.RegisterRoute(nameof(CreateNewAccount), typeof(CreateNewAccount));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(YourComplaint), typeof(YourComplaint));
            Routing.RegisterRoute(nameof(ComplaintPage), typeof(ComplaintPage));
        }
    }
}
