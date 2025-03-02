using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MauiApp3
{
    public partial class ComplaintPage : ContentPage
    {
        private IMongoCollection<BsonDocument> _complaintsCollection;
        private double _latitude;
        private double _longitude;
        private byte[] _imageData;

        // Local MongoDB connection string
        const string mongoUri = "mongodb://localhost:27017";

        public ComplaintPage()
        {
            InitializeComponent();
            
        }


        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            try
            {
                if (_complaintsCollection == null)
                {
                    await DisplayAlert("Error", "Database connection failed.", "OK");
                    return;
                }

                string name = FullNameEntry?.Text?.Trim() ?? "";
                string mobileNumber = MobileEntry?.Text?.Trim() ?? "";
                string description = ComplaintEditor?.Text?.Trim() ?? "";

                string category = (GarbageBorder?.BackgroundColor == Colors.LightGray) ? "Garbage" :
                                  (RoadDamageBorder?.BackgroundColor == Colors.LightGray) ? "Road Damage" : "";

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mobileNumber) ||
                    string.IsNullOrEmpty(category) || string.IsNullOrEmpty(description))
                {
                    await DisplayAlert("Validation", "Please fill in all the required fields.", "OK");
                    return;
                }

                if (!long.TryParse(mobileNumber, out _) || mobileNumber.Length != 10)
                {
                    await DisplayAlert("Validation", "Please enter a valid 10-digit mobile number.", "OK");
                    return;
                }

                var complaint = new BsonDocument
                {
                    { "name", name },
                    { "mobileNumber", mobileNumber },
                    { "description", description },
                    { "image", _imageData != null ? new BsonBinaryData(_imageData) : BsonNull.Value },
                    { "latitude", _latitude },
                    { "longitude", _longitude },
                    { "category", category },
                    { "status", "pending" },
                    { "isApproved", false },
                    { "location", new BsonDocument {
                        { "type", "Point" },
                        { "coordinates", new BsonArray { _longitude, _latitude } }
                    }},
                    { "timestamp", DateTime.UtcNow }
                };

                //await _complaintsCollection.InsertOneAsync(complaint);
                await DisplayAlert("Success", "Your complaint has been submitted successfully.", "OK");
                ResetForm();
            }
            catch (Exception ex)
            {
                
            }
        }

        [Obsolete]
        private async void OnUploadImageTapped(object sender, EventArgs e)
        {
            try
            {
                var locationPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                var cameraPermission = await Permissions.RequestAsync<Permissions.Camera>();

                if (locationPermission != PermissionStatus.Granted || cameraPermission != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission Required", "Both Camera and Location permissions are needed.", "OK");
                    return;
                }

                await ValidateLocation();

                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null) return;

                using (var stream = await photo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    _imageData = memoryStream.ToArray();
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    SelectedImage.Source = ImageSource.FromStream(() => new MemoryStream(_imageData));
                    UploadLabel.IsVisible = false;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
            }
        }

        private async Task ValidateLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                Location location = await Geolocation.GetLocationAsync(request);

                if (location == null)
                {
                    await DisplayAlert("Location Error", "Could not get location. Please try again.", "OK");
                    return;
                }

                _latitude = location.Latitude;
                _longitude = location.Longitude;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
            }
        }

        private void ResetForm()
        {
            FullNameEntry.Text = string.Empty;
            MobileEntry.Text = string.Empty;
            ComplaintEditor.Text = string.Empty;
            SelectedImage.Source = null;
            UploadLabel.IsVisible = true;
            _latitude = 0;
            _longitude = 0;
            _imageData = null;
        }

        private void OnGarbageSelected_Tapped(object sender, EventArgs e)
        {
            GarbageBorder.BackgroundColor = Colors.LightGray;
            RoadDamageBorder.BackgroundColor = Colors.White;
        }

        private void OnRoadDamageSelected_Tapped(object sender, EventArgs e)
        {
            RoadDamageBorder.BackgroundColor = Colors.LightGray;
            GarbageBorder.BackgroundColor = Colors.White;
        }

        private async void Home_Clicked(object sender, EventArgs e)
        {
            if (!Navigation.NavigationStack.Any(p => p is MainPage))
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

        private async void Complaint_Clicked(object sender, EventArgs e)
        {
            if (!Navigation.NavigationStack.Any(p => p is ComplaintPage))
            {
                await Navigation.PushAsync(new ComplaintPage());
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Profile_Clicked(object sender, EventArgs e)
        {
            if (!Navigation.NavigationStack.Any(p => p is ProfilePage))
            {
                await Navigation.PushAsync(new ProfilePage());
            }
        }
    }
}
