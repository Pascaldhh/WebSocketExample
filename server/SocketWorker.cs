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
            foreach (Player player in Game.Players.ToList())
            {
                if(player.WS.State != WebSocketState.Open) CloseSocket(player.WS, player);
                await SendJsonObject(player.WS, new SendData(SendType.GameInfo, Game));
            }

            await Task.Delay(1000 / 120, stoppingToken); // 60fps logic loop
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
            RecieveData? data = JsonSerializer.Deserialize<RecieveData>(ms, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
            if(data?.Data == null) return;

            switch (data.Type)
            {
                case RecieveType.Init:
                    Player newPlayer = new(ws, 100, 100, 30, 60, data.Data.Name, data.Data.Color);
                    Game.Players.Add(newPlayer);
                    await SendJsonObject(ws, new SendData(SendType.InitConfirm, new InitConfirmData(true)));
                    break;

                case RecieveType.Movement:
                    Player player = Game.Players.First(player => player.WS.Equals(ws));

                    switch (data.Data.Movement)
                    {
                        case PlayerMovement.Left:
                            player.Movement = PlayerMovement.Left;
                            break;
                        case PlayerMovement.Right:
                            player.Movement = PlayerMovement.Right;
                            break;
                        case PlayerMovement.Idle:
                            player.Movement = PlayerMovement.Idle;
                            break;
                    }

                    if (data.Data.Jump && player.State != PlayerState.InAir)
                    {
                        player.VelocityY = -5;
                        player.State = PlayerState.InAir;
                    }
                    break;
            }
        }
    }

    public async Task SendJsonObject(WebSocket[] wss, object obj)
    {
        foreach (WebSocket ws in wss)
            await SendJsonObject(ws, obj);
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

    public async void CloseSocket(WebSocket ws, Player player)
    {
        Game.Players.Remove(player);
        CloseSocket(ws);
    }
    public async void CloseSocket(WebSocket ws)
    {
        Connections.Remove(ws);
    }
}