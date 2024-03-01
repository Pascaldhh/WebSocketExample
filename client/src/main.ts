import {drawPlayer, Player} from "./entities/Player.js";
import { canvas, ctx } from "./entities/Canvas.js";
import { Socket } from "./entities/WebSocket.js";
import { SendData, SendType } from "./data/SendData.js";
import { RecieveData, RecieveType } from "./data/RecieveData.js";

class Game {
  public players : Player[];
  constructor() {
    this.players = [];
    this.socketEvents();
    this.events();
  }

  start() {
    document?.getElementById("screen-init")?.classList.add("closed");
    requestAnimationFrame(this.draw.bind(this));
  }

  draw() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    this.players?.forEach(player => drawPlayer(player));
    requestAnimationFrame(this.draw.bind(this));
  }

  socketEvents() {
    Socket.response((message) => {
      const recieveData : RecieveData = JSON.parse(message.data);
      switch (recieveData.type) {
        case RecieveType.InitConfirm:
          this.players = recieveData.data?.game?.players;
          this.start();
          break;
        case RecieveType.GameInfo:
          this.players = recieveData.data?.players;
          this.setPlayerCount(this.players.length);
          break;
      }
    });
  }

  setPlayerCount(n : number) {
    const countElement = document.getElementById("player-count");
    if(countElement == null) return;
    countElement.textContent = `${n}`;
  }

  events() {
    document.getElementById("start-form")?.addEventListener("submit", (e : SubmitEvent) => {
      e.preventDefault();
      const data = new FormData(e.currentTarget as HTMLFormElement);
      Socket.send(JSON.stringify(new SendData(SendType.init, {"name": data.get("name"), "color": data.get("color")})));
    });
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

  keyEvents() {
    addEventListener("keydown", this.startMoving.bind(this));
    addEventListener("keyup", this.endMoving.bind(this));
  }
}

new Game();
