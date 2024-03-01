export enum RecieveType { InitConfirm, GameInfo }

export class RecieveData {
  public type: RecieveType;
  public data: object;

  constructor(type : RecieveType, data : object) {
    this.type = type;
    this.data = data;
  }
}