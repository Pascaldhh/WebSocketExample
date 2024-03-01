type Action = (message : MessageEvent<any>) => void;

class WSocket {
  public instance;
  constructor(url: string) {
    this.instance = new WebSocket(url);
    this.instance.onerror = () => document.getElementById("screen-disconnect")?.classList.add("show");
  }
  response(callback : Action) {
    if(this.instance == null) return;
    this.instance.addEventListener("message", (message) => {
      callback(message);
    })
  }
  send(data : string | ArrayBufferLike | Blob | ArrayBufferView) {
    if(this.instance == null) return;
    this.instance.send(data);
  }
}
export const Socket: WSocket = new WSocket("ws://localhost:6969/");
