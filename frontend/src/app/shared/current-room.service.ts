import { Injectable } from '@angular/core';
import { Room } from './room.model';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  private _rooms: Room[] = [];
  private _currentRoom: Room = new Room(
    101,
    true,
    100,
    'Single',
    'Single room with a single bed',
    ['megmondtam', 'tdanny'],
    ['https://via.placeholder.com/150', 'https://via.placeholder.com/150']
  );

  public get currentRoom(): Room {
    return this._currentRoom;
  }

  public get rooms(): Room[] {
    return this._rooms;
  }

  public set currentRoom(value: Room) {
    this._currentRoom = value;
  }

  public set rooms(value: Room[]) {
    this._rooms = value;
  }

  constructor() {}
}
