import { canvas, canvasHeight, ctx } from "./Canvas.js";
import { round } from "./Utils.js";

enum PlayerMovement { Idle,  Left, Right }
enum PlayerState { InAir, OnGround }

export default class Player {
  private x : number;
  private y : number;

  private velocityX : number;
  private velocityY : number;

  private width: number;
  private height : number;
  private name : string;
  private color : string;

  private state : PlayerState;
  private movement : PlayerMovement;

  constructor(x : number, y : number, width : number, height : number, name: string, color : string) {
    this.x = x;
    this.y = y
    this.velocityX = 0;
    this.velocityY = 0;
    this.width = width;
    this.height = height;
    this.name = name;
    this.color = color;
    this.state = PlayerState.InAir;
    this.movement = PlayerMovement.Idle;
    this.events();
  }
  logic() {
    this.stateLogic();
    this.movementLogic();

    this.x += this.velocityX;
    this.y += this.velocityY;
  }

  stateLogic() {
    if((this.y + this.height + this.velocityY) >= canvasHeight) {
      this.velocityY = canvasHeight - (this.y + this.height);
      this.y += this.velocityY;
      this.state = PlayerState.OnGround;
    }
    switch (this.state) {
      case PlayerState.OnGround:
        this.velocityY = 0;
        break;
      case PlayerState.InAir:
        this.velocityY += .14;
        break;
    }
  }

  movementLogic() {
    switch (this.movement) {
      case PlayerMovement.Right:
        if(this.velocityX < 1.5)
          this.velocityX += .1;
        else this.velocityX = 1.5;
        break;
      case PlayerMovement.Left:
        if(this.velocityX > -1.5)
          this.velocityX += -.1;
        else this.velocityX = -1.5;
        break;
      case PlayerMovement.Idle:
        if(round(this.velocityX) === 0) {
          console.log("Stopped")
          this.velocityX = 0;
        }
        if(round(this.velocityX) < 0)
          this.velocityX += .1;
        if(round(this.velocityX) > 0)
          this.velocityX += -.1;
        break;
    }
  }

  draw() {
    const oldStyle= ctx.fillStyle;

    // Draw name
    ctx.font = "14px apple";
    const textCenterX = this.x + (this.width / 2) - ctx.measureText(this.name).width / 2;
    ctx.fillText(this.name, textCenterX, this.y-10);

    // Draw square
    ctx.fillStyle = this.color;
    ctx.fillRect(this.x, this.y, this.width, this.height);
    ctx.fillStyle = oldStyle;
  }

  startMoving(event : KeyboardEvent) {
    switch (event.code) {
      case "KeyA":
        this.movement = PlayerMovement.Left;
        break;
      case "KeyD":
        this.movement = PlayerMovement.Right;
        break;
      case "KeyW":
      case "Space":
        if(this.state != PlayerState.InAir) {
          this.velocityY = -5;
          this.state = PlayerState.InAir;
        }
        break;
    }
  }

  endMoving(event : KeyboardEvent) {
    switch (event.code) {
      case "KeyA":
      case "KeyD":
        this.movement = PlayerMovement.Idle;
        break;
    }

  }

  events() {
    addEventListener("keydown", this.startMoving.bind(this));
    addEventListener("keyup", this.endMoving.bind(this));
  }
}