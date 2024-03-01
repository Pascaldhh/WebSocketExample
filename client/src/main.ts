import {drawPlayer, Player, PlayerMovement} from "./entities/Player.js";
import {canvas, ctx} from "./entities/Canvas.js";
import {Socket} from "./entities/WebSocket.js";
import {SendData, SendType} from "./data/SendData.js";
import {RecieveData, RecieveType} from "./data/RecieveData.js";

class Game {
  public players : Player[];
  constructor() {
    this.players = [];
    this.socketEvents();
    this.events();
  }

  start() {
    document?.getElementById("screen-init")?.classList.add("closed");
    (document.getElementById("leave") as HTMLButtonElement).disabled = false;
    this.addKeyEvents();
    requestAnimationFrame(this.draw.bind(this));
  }

  stop() {
    window.location.reload();
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
      Socket.send(JSON.stringify(new SendData(SendType.Init, {"name": data.get("name"), "color": data.get("color")})));
    });
    document.getElementById("leave")?.addEventListener("click", () => this.stop());
  }

  startMoving(event : KeyboardEvent) {
    switch (event.code) {
      case "KeyA":
        Socket.send(JSON.stringify(new SendData(SendType.Movement, { "movement": PlayerMovement.Left })));
        break;
      case "KeyD":
        Socket.send(JSON.stringify(new SendData(SendType.Movement, { "movement": PlayerMovement.Right })));
        break;
      case "Space":
        Socket.send(JSON.stringify(new SendData(SendType.Movement, { "jump": true })));
        break;
    }
  }

  endMoving(event : KeyboardEvent) {
    switch (event.code) {
      case "KeyA":
      case "KeyD":
        Socket.send(JSON.stringify(new SendData(SendType.Movement, { "movement": PlayerMovement.Idle })));
    }
  }

  addKeyEvents() {
    addEventListener("keydown", this.startMoving.bind(this));
    addEventListener("keyup", this.endMoving.bind(this));
  }

  removeKeyEvents() {
    removeEventListener("keydown", this.startMoving.bind(this));
    removeEventListener("keyup", this.endMoving.bind(this));
  }
}

new Game();
