export enum SendType { Init, Movement }

export class SendData {
  public type: SendType;
  public data: object;

  constructor(type : SendType, data : object) {
    this.type = type;
    this.data = data;
  }
}