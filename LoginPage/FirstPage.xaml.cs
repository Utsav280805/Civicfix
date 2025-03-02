using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp3;

public partial class FirstPage : ContentPage
{
    public FirstPage()
    {
        InitializeComponent();
        AnimateElements();
    }

    private async void AnimateElements()
    {
        await TitleLabel.FadeTo(1, 1200, Easing.CubicInOut);
        await TitleLabel.ScaleTo(1, 1000, Easing.CubicOut);

        await Task.Delay(300);

        await Task.WhenAll(
            Tagline1.FadeTo(1, 800, Easing.CubicInOut),
            Tagline1.TranslateTo(0, -5, 800, Easing.CubicOut)
        );

        await Task.Delay(200);

        await Task.WhenAll(
            Tagline2.FadeTo(1, 800, Easing.CubicInOut),
            Tagline2.TranslateTo(0, -5, 800, Easing.CubicOut)
        );

        await Task.Delay(200);

        await Task.WhenAll(
            Tagline3.FadeTo(1, 800, Easing.CubicInOut),
            Tagline3.TranslateTo(0, -5, 800, Easing.CubicOut)
        );

        await Task.Delay(1000); // Smooth pause before navigating

        await Shell.Current.GoToAsync("//LoginPage"); // Ensure MainPage is registered
    }
}
