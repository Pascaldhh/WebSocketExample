using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using server.data;

namespace server;

public class SocketWorker : BackgroundService
{
    public List<WebSocket> Connections { get; set; } = new();
    public Game Game { get; } = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Game.Loop();
            foreach (Player player in Game.Players)
                await SendJsonObject(player.WS, new SendData(SendType.GameInfo, Game));

            await Task.Delay(1000 / 60, stoppingToken); // 60fps logic loop
        }
    }

    public async Task Recieve(WebSocket ws)
    {
        byte[] buffer = new byte[1024 * 4];
        while (ws.State.HasFlag(WebSocketState.Open))
        {
            WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType != WebSocketMessageType.Text)
            {
                CloseSocket(ws);
                return;
            }
            using MemoryStream ms = new(buffer, 0, result.Count);
            RecieveInitData? initPlayerData = JsonSerializer.Deserialize<RecieveInitData>(ms, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
            if(initPlayerData?.Data == null) return;
            Game.Players.Add(new Player(ws, 100, 100, 30, 60, initPlayerData.Data.Name, initPlayerData.Data.Color));
            await SendJsonObject(ws, new SendData(SendType.InitConfirm, new InitConfirmData{ IsReady = true }));
        }
    }

    public async Task SendJsonObject(WebSocket ws, object obj)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(obj, new JsonSerializerOptions{ PropertyNamingPolicy = new LowerCaseNamingPolicy() });
        ArraySegment<byte> arraySegment = new(bytes, 0, bytes.Length);
        if (ws.State == WebSocketState.Open)
            await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        else if (ws.State is WebSocketState.Closed or WebSocketState.Aborted)
            CloseSocket(ws);
    }

    public async void CloseSocket(WebSocket ws)
    {
        Connections.Remove(ws);
        await ws.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
    }
}