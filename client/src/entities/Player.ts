import { canvas, canvasHeight, ctx } from "./Canvas.js";

export default class Player {
  private x : number;
  private y : number;

  private width: number;
  private height : number;
  private name : string;
  private color : string;

  constructor(x : number, y : number, width : number, height : number, name: string, color : string) {
    this.x = x;
    this.y = y
    this.width = width;
    this.height = height;
    this.name = name;
    this.color = color;
  }

  setValues(x : number, y : number, width : number, height : number, name: string, color : string) {
    this.x = x;
    this.y = y
    this.width = width;
    this.height = height;
    this.name = name;
    this.color = color;
  }

  draw() {
    const oldStyle= ctx.fillStyle;

    // Draw name
    ctx.font = "14px Arial";
    const textCenterX = this.x + (this.width / 2) - ctx.measureText(this.name).width / 2;
    ctx.fillText(this.name, textCenterX, this.y-10);

    // Draw square
    ctx.fillStyle = "black";
    ctx.fillRect(this.x-2, this.y-2, this.width+4, this.height+4);
    ctx.fillStyle = this.color;
    ctx.fillRect(this.x, this.y, this.width, this.height);
    ctx.fillStyle = oldStyle;
  }

  startMoving(event : KeyboardEvent) {
    switch (event.code) {
      case "KeyA":
        break;
      case "KeyD":
        break;
      case "KeyW":
      case "Space":
        break;
    }
  }

  endMoving(event : KeyboardEvent) {
    switch (event.code) {
      case "KeyA":
      case "KeyD":
    }
  }

  events() {
    addEventListener("keydown", this.startMoving.bind(this));
    addEventListener("keyup", this.endMoving.bind(this));
  }
}