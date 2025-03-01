using CivicFix.Views;

namespace CivicFix
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(CreateNewAccount), typeof(CreateNewAccount));
            Routing.RegisterRoute(nameof(UserDashbord), typeof(UserDashbord));
            Routing.RegisterRoute(nameof(RAdminDashboard), typeof(RAdminDashboard));
            Routing.RegisterRoute(nameof(GAdminDashboard), typeof(GAdminDashboard));

        }
    }
}
