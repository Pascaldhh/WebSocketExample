import Player from "./Player.js";
import { canvas, ctx } from "./Canvas.js";
import { Socket } from "./WebSocket.js";
Socket.instance.addEventListener("open", () => {
  console.log(`[websockets] Connected to ${Socket.instance.url}`);
});

Socket.instance.addEventListener("message", (message) => {
  console.log(message.data)
})

class Game {
  private players : Player[];
  constructor() {
    this.players = [new Player(40, 40, 25, 40, "Pascal", "red")];
    this.start();
  }

  start() {
    setInterval(this.logic.bind(this), 1000/60);
    requestAnimationFrame(this.draw.bind(this));
  }

  draw() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    this.players.forEach(player => player.draw());

    requestAnimationFrame(this.draw.bind(this));
  }

  logic() {
    this.players.forEach(player => player.logic());
  }
}

new Game();
