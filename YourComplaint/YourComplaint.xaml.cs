using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp3;

public partial class YourComplaint : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    public ObservableCollection<Complaint> Complaints { get; set; } = new ObservableCollection<Complaint>();

    public YourComplaint()
    {
        InitializeComponent();
        ComplaintsListView.ItemsSource = Complaints;
        _ = LoadComplaints(); // Run async method
    }

    private async Task LoadComplaints()
    {
        try
        {
            string apiUrl = "https://your-api.com/getComplaints"; // Replace with your actual API URL
            string response = await _httpClient.GetStringAsync(apiUrl);

            var data = JsonSerializer.Deserialize<ComplaintResponse>(response);

            if (data != null)
            {
                Complaints.Clear();
                foreach (var complaint in data.Complaints)
                {
                    Complaints.Add(complaint);
                }

                // Update UI safely
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PendingComplaintsLabel.Text = data.PendingCount.ToString();
                    CompletedComplaintsLabel.Text = data.CompletedCount.ToString();
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load complaints: {ex.Message}", "OK");
        }
    }

    private async void AddComplaint_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ComplaintPage"); // Navigate to Add Complaint Page
    }
}

public class Complaints
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
}

public class ComplaintResponse
{
    [JsonPropertyName("pending_count")]
    public int PendingCount { get; set; }

    [JsonPropertyName("completed_count")]
    public int CompletedCount { get; set; }

    [JsonPropertyName("complaints")]
    public Complaint[] Complaints { get; set; }
}
