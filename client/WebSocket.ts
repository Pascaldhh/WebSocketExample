class WSocket {
  public instance;
  constructor(url: string) {
    this.instance = new WebSocket(url);
  }
}
export const Socket: WSocket = new WSocket("ws://localhost:6969/");