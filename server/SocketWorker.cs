using System.Net.WebSockets;
using System.Text.Json;

namespace server;

public class SocketWorker : BackgroundService
{
    public List<WebSocket> Connections { get; set; }
    public Game Game { get; } = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Game.Loop();
            await Task.Delay(1000 / 60, stoppingToken);
        }
    }

    public async Task SendGameStatus(WebSocket ws)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(this);
        ArraySegment<byte> arraySegment = new(bytes, 0, bytes.Length);
        if (ws.State == WebSocketState.Open)
            await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        // else if(ws.State is WebSocketState.Closed or WebSocketState.Aborted) return;
    }
}