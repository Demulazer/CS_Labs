using TestLib;

namespace Tests;

public class Start
{
    public static async Task Main(string[] args)
    {
        var songView = new SongView();
        await songView.InitializeMenu();
    }
}