import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataStorageService } from '../shared/data-storage.service';
import { Subscription } from 'rxjs';
import { Room } from '../shared/room.model';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css',
})
export class RoomsComponent implements OnInit, OnDestroy {
  getRoomsSubscribtion = new Subscription();
  getRoomSubscribtion = new Subscription();
  hoveredItem?: Room | null;
  rooms: Room[] = [];
  isLoading = true;

  constructor(private http: DataStorageService) {}
  ngOnInit(): void {
    this.hoveredItem = null;
    this.getRoomsSubscribtion = this.http.getRooms().subscribe((data) => {
      this.rooms = data;
      console.log(this.rooms);
    });

    // this.getRoomSubscribtion = this.http.getRoom(101).subscribe((data) => {
    //   console.log(data);
    // });
    this.isLoading = false;
  }

  onSetRoom(item: Room | null) {
    this.hoveredItem = item;
  }

  ngOnDestroy(): void {
    this.getRoomsSubscribtion.unsubscribe();
    this.getRoomSubscribtion.unsubscribe();
  }
}
