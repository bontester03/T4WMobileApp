using Android.App;
//using Android.Content.PM;
//using Android.OS;

//namespace T4sV1
//{
//    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
//    public class MainActivity : MauiAppCompatActivity
//    {
//    }
//}
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace T4sV1
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set status bar color to match your app theme
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor("#b8db88"));

                // Set status bar icons/text to dark color for better visibility on light background
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var uiOptions = (int)Window.DecorView.SystemUiVisibility;
                    uiOptions |= (int)SystemUiFlags.LightStatusBar;
                    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
                }
            }
        }
    }
}