import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataStorageService } from '../shared/data-storage.service';
import { Subscription } from 'rxjs';
import { Room } from '../shared/room.model';
import { RoomService } from '../shared/current-room.service';

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
    private roomService: RoomService
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
      this.roomService.rooms = this.rooms;
      console.log(this.rooms);
    });

    // this.getRoomSubscribtion = this.http.getRoom(101).subscribe((data) => {
    //   console.log(data);
    // });

    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }

  onSetRoom(item: Room) {
    this.roomService.currentRoom = item;
  }

  ngOnDestroy(): void {
    this.getRoomsSubscribtion.unsubscribe();
    this.getRoomSubscribtion.unsubscribe();
  }
}
