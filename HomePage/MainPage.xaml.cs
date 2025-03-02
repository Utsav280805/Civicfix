using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {
        public class Complaint
        {
            public int Id { get; set; } // Unique identifier for the complaint
            public string Category { get; set; } // Complaint category (Garbage, Road Damage, etc.)
            public string Status { get; set; }  // Complaint status (Pending, Solved, etc.)
        }
        private List<Complaint> _complaints; // Local list to store complaints

        public MainPage()
        {
            InitializeComponent();
            LoadComplaints();
            UpdateSolvedComplaints();
        }

        // Initialize complaints list (only Garbage & Road Damage)
        private void LoadComplaints()
        {
            _complaints = new List<Complaint>
            {
                new Complaint { Id = 1, Category = "Road Damage", Status = "Solved" },
                new Complaint { Id = 2, Category = "Garbage", Status = "Pending" },
                new Complaint { Id = 3, Category = "Road Damage", Status = "Solved" },
                new Complaint { Id = 4, Category = "Garbage", Status = "Solved" }
            };
        }

        // Count and update solved complaints (only for Garbage & Road Damage)
        private void UpdateSolvedComplaints()
        {
            int solvedCount = _complaints.Count(c => c.Status == "Solved" &&
                                                     (c.Category == "Garbage" || c.Category == "Road Damage"));

            SolvedComplaintsLabel.Text = $"{solvedCount}";
        }

        // "Complain Here" button click
        private async void complaint_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ComplaintPage());
        }

        // "Your Complaint" button click
        private async void Profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        // "Home" button click
        private async void Home_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
        private async void YourComplaintPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new YourComplaint());
        }
    }

    // Complaint Model (with Category)
    

}
