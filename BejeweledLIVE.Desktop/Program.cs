using Sexy;

namespace BejeweledLIVE.Desktop;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        using var game = new Sexy.Main();
        game.Run();
    }
}
