using Foundation;
using UIKit;

namespace BejeweledLIVE.iOS;

[Register("AppDelegate")]
internal class Program : UIApplicationDelegate
{
    private static Sexy.Main game;
    static void Main(string[] args) => UIApplication.Main(args, null, typeof(Program));
    public override void FinishedLaunching(UIApplication app)
    {
        game = new Sexy.Main();
        game.Run();
    }
}
