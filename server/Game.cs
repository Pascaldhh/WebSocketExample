using System.Net.WebSockets;
using System.Text.Json;

namespace server;

public static class GameInfo
{
    public static int Width = 1200;
    public static int Height = 700;
}
public class Game
{
    public int Width { get; set; } = GameInfo.Width;
    public int Height { get; set; } = GameInfo.Height;
    public List<Player> Players { get; set; } = [];
    public void Loop()
    {
        foreach (Player player in Players)
            player.Logic();
    }
}