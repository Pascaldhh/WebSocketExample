import { canvas, canvasHeight, ctx } from "./Canvas.js";
export enum PlayerMovement { Idle,  Left, Right }

export type Player = {
  id: number;
  name: string;
  color: string;
  x: number;
  y: number;
  width: number;
  height: number;
}

export function drawPlayer(player : Player) {
  const oldStyle= ctx.fillStyle;

  // Draw name
  ctx.font = "14px Arial";
  const textCenterX = player.x + (player.width / 2) - ctx.measureText(player.name).width / 2;
  ctx.fillText(player.name, textCenterX, player.y-10);

  // Draw square
  ctx.fillStyle = "black";
  ctx.fillRect(player.x-2, player.y-2, player.width+4, player.height+4);
  ctx.fillStyle = player.color;
  ctx.fillRect(player.x, player.y, player.width, player.height);
  ctx.fillStyle = oldStyle;
}
