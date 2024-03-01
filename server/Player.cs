using System.Net.WebSockets;
using System.Text.Json.Serialization;

namespace server;

public enum PlayerMovement { Idle,  Left, Right }
public enum PlayerState { InAir, OnGround }

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
    public double VelocityY { get; set; }

    public PlayerState State { get; set; } = PlayerState.InAir;
    public PlayerMovement Movement { get; set; } = PlayerMovement.Idle;

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

    public string? Error()
    {
        if (Name.Length > 50 || Color.Length > 50) return "Input fields can't be longer than 50!";
        if (Name.Length < 5) return "Name must be longer than 4 chars";
        return null;
    }

    public void Logic()
    {
        StateLogic();
        MovementLogic();
        Collision();

        X += VelocityX;
        Y += VelocityY;
    }

    private void Collision()
    {
        if (X <= 0)
        {
            Movement = PlayerMovement.Idle;
            VelocityX = 0;
            X = 1;
        }

        if (X + Width >= GameInfo.Width)
        {
            Movement = PlayerMovement.Idle;
            VelocityX = 0;
            X = GameInfo.Width - Width - 1;
        }
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