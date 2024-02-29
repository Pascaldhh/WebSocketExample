type Action = (message : MessageEvent<any>) => void;

class WSocket {
  public instance;
  constructor(url: string) {
    this.instance = new WebSocket(url);
  }
  response(callback : Action) {
    Socket.instance.addEventListener("message", (message) => {
      callback(message);
    })
  }
  send(data : string | ArrayBufferLike | Blob | ArrayBufferView) {
    Socket.instance.send(data);
  }
}
export const Socket: WSocket = new WSocket("ws://localhost:6969/");
