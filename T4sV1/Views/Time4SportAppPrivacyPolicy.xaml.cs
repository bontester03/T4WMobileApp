namespace T4sV1.Views;

public partial class Time4SportAppPrivacyPolicy : ContentPage
{
    public Time4SportAppPrivacyPolicy()
    {
        InitializeComponent();
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

