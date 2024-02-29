import Player from "./entities/Player.js";
import {canvas, ctx} from "./entities/Canvas.js";
import {Socket} from "./entities/WebSocket.js";
import {SendData, SendType} from "./entities/Data.js";

class Game {
  private players : Player[];
  constructor() {
    this.players = [];
    this.events();
  }

  start() {
    this.update();
    requestAnimationFrame(this.draw.bind(this));
  }

  draw() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    this.players.forEach(player => player.draw());

    requestAnimationFrame(this.draw.bind(this));
  }

  update() {
    Socket.response((message) => {

    });
  }

  events() {
    document.getElementById("start-form")?.addEventListener("submit", (e : SubmitEvent) => {
      e.preventDefault();
      const data = new FormData(e.currentTarget as HTMLFormElement);
      Socket.send(JSON.stringify(new SendData(SendType.init, data.entries())));
    });
  }
}

new Game();
