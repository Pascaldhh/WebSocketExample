using System.Net.WebSockets;
using System.Text.Json.Serialization;

namespace server;

enum PlayerMovement { Idle,  Left, Right }
enum PlayerState { InAir, OnGround }

public class Player
{
    private static int _idCount;
    [JsonIgnore]
    public WebSocket WS { get; }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    private double VelocityX { get; set; }
    private double VelocityY { get; set; }

    private PlayerState State { get; set; } = PlayerState.InAir;
    private PlayerMovement Movement { get; set; } = PlayerMovement.Idle;

    public Player(WebSocket ws, double x, double y, int width, int height, string name, string color)
    {
        Id = _idCount;
        _idCount++;
        WS = ws;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Name = name;
        Color = color;
    }

    public void Logic()
    {
        StateLogic();
        MovementLogic();

        X += VelocityX;
        Y += VelocityY;
    }

    private void StateLogic()
    {
        if((Y + Height + VelocityY) >= GameInfo.Height) {
            VelocityY = GameInfo.Height - (Y + Height);
            Y += VelocityY;
            State = PlayerState.OnGround;
        }
        switch (State) {
            case PlayerState.OnGround:
                VelocityY = 0;
                break;
            case PlayerState.InAir:
                VelocityY += .14;
                break;
        }
    }

    private void MovementLogic()
    {
        switch (Movement) {
            case PlayerMovement.Right:
                if(VelocityX < 1.5)
                    VelocityX += .1;
                else VelocityX = 1.5;
                break;
            case PlayerMovement.Left:
                if(VelocityX > -1.5)
                    VelocityX += -.1;
                else VelocityX = -1.5;
                break;
            case PlayerMovement.Idle:
                if(VelocityX == 0)
                    VelocityX = 0;
                if(VelocityX < 0)
                    VelocityX += .1;
                if(VelocityX > 0)
                    VelocityX += -.1;
                break;
        }
    }
}