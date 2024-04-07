import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataStorageService } from '../shared/data-storage.service';
import { Subscription } from 'rxjs';
import { Room } from '../shared/room.model';
import { CurrentRoomService } from '../shared/current-room.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css',
})
export class RoomsComponent implements OnInit, OnDestroy {
  getRoomsSubscribtion = new Subscription();
  getRoomSubscribtion = new Subscription();
  rooms: Room[] = [];
  isLoading = true;

  constructor(
    private http: DataStorageService,
    private currentRoom: CurrentRoomService
  ) {}
  ngOnInit(): void {
    this.getRoomsSubscribtion = this.http.getRooms().subscribe((data) => {
      this.rooms = data;
      this.rooms.forEach((room) => {
        room.images.forEach((image, index) => {
          if (!image.startsWith('http')) {
            room.images[index] = '../../../assets/img/gyobos_placeholder.jpg';
          }
        });
      });
      console.log(this.rooms);
    });

    // this.getRoomSubscribtion = this.http.getRoom(101).subscribe((data) => {
    //   console.log(data);
    // });
    this.isLoading = false;
  }

  onSetRoom(item: Room) {
    this.currentRoom.currentRoom = item;
  }

  ngOnDestroy(): void {
    this.getRoomsSubscribtion.unsubscribe();
    this.getRoomSubscribtion.unsubscribe();
  }
}
