using System.Net;
using System.Net.WebSockets;
using server;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");

builder.Services.AddSingleton<SocketWorker>();
builder.Services.AddHostedService<SocketWorker>(p => p.GetRequiredService<SocketWorker>());

var app = builder.Build();
app.UseWebSockets();
app.Map("/", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        app.Services
            .GetService<SocketWorker>()?
            .Connections
            .Add(ws);

        // To keep websocket alive
        while (ws.State.HasFlag(WebSocketState.Open)) {}
    }
    else context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
});

await app.RunAsync();