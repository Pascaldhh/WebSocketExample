using System.Net.WebSockets;

namespace server;

enum PlayerMovement { Idle,  Left, Right }
enum PlayerState { InAir, OnGround }

public class Player(WebSocket ws, double x, double y, int width, int height, string name, string color)
{
    public string Name { get; set; } = name;
    public string Color { get; set; } = color;
    public double X { get; set; } = x;
    public double Y { get; set; } = y;

    public int Width { get; set; } = width;
    public int Height { get; set; } = height;

    private double VelocityX { get; set; }
    private double VelocityY { get; set; }

    private PlayerState State { get; set; } = PlayerState.InAir;
    private PlayerMovement Movement { get; set; } = PlayerMovement.Idle;

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