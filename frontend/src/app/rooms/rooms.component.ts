import { Component, OnInit } from '@angular/core';
import { DataStorageService } from '../shared/data-storage.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css',
})
export class RoomsComponent implements OnInit {
  constructor(private http: DataStorageService) {}
  ngOnInit(): void {
    this.http.getRooms().subscribe((data) => {
      console.log(data);
    });
  }
}
