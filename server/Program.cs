using System.Net;
using System.Net.WebSockets;
using server;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");
// builder.Services.Configure<HostOptions>(hostOptions =>
// {
//     hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
// });
// Add in dependency injection and add BackgroundService
builder.Services.AddSingleton<SocketWorker>();
builder.Services.AddHostedService<SocketWorker>(p => p.GetRequiredService<SocketWorker>());

var app = builder.Build();
app.UseWebSockets();

app.Map("/", async context =>
{
    SocketWorker? socketWorker = app.Services.GetService<SocketWorker>();
    if (socketWorker != null && context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        socketWorker.Connections.Add(ws);

        // To recieve websocket messages
        await socketWorker.Recieve(ws);
    }
    else context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
});

await app.RunAsync();