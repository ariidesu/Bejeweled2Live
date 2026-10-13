using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace BejeweledLIVE.Android;

[Activity(Label = "Bejeweled LIVE", MainLauncher = true, Icon = "@drawable/icon",
    Theme = "@style/Theme.Splash", AlwaysRetainTaskState = true,
    LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.FullSensor,
    ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize,
    Immersive = true, HardwareAccelerated = true)]
public class Activity1 : AndroidGameActivity
{
    private Sexy.Main game;
    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);
        game = new Sexy.Main();
        var view = (View)game.Services.GetService(typeof(View));
        view.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutStable | SystemUiFlags.LayoutHideNavigation |
            SystemUiFlags.LayoutFullscreen | SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen | SystemUiFlags.ImmersiveSticky);
        SetContentView(view);
        game.Run();
    }
}
