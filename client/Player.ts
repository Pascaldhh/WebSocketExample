import {canvas, canvasHeight, ctx} from "./Canvas.js";

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
    this.movement = PlayerMovement.Idle;
    this.state = PlayerState.InAir;
    this.events();
  }
  logic() {
    if((this.y + this.height + this.velocityY) >= canvasHeight) {
      this.velocityY = canvasHeight - (this.y + this.height);
      console.log(this.y, this.velocityY);
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

    this.x += this.velocityX;
    this.y += this.velocityY;
  }

  draw() {
    const oldStyle= ctx.fillStyle;

    // Draw name
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
    this.movement = PlayerMovement.Idle;
  }

  events() {
    addEventListener("keydown", this.startMoving.bind(this));
    addEventListener("keyup", this.endMoving.bind(this));
  }
}