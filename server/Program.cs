using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");

var app = builder.Build();

app.UseWebSockets();
app.Map("/", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        while (true)
        {
            string message = "The current time is: " + DateTime.Now.ToString("HH:mm:ss");
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            ArraySegment<byte> arraySegment = new(bytes, 0, bytes.Length);
            if (ws.State == WebSocketState.Open)
                await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            else if(ws.State is WebSocketState.Closed or WebSocketState.Aborted) break;
            Thread.Sleep(1000/60);
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});
await app.RunAsync();