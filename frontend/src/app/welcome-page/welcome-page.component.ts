import { Component, OnInit } from '@angular/core';
import { DataStorageService } from '../shared/data-storage.service';
import { Subscription, timeout } from 'rxjs';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.component.html',
  styleUrl: './welcome-page.component.css',
})
export class WelcomePageComponent implements OnInit {
  getRoomsSubscription = new Subscription();

  constructor(private dataStorage: DataStorageService) {}

  ngOnInit() {
    this.getRoomsSubscription = this.dataStorage
      .getRooms()
      .pipe(
        timeout(5000) // Set timeout to 5 seconds (adjust as needed)
      )
      .subscribe(
        (data) => {
          console.log(data);
        },
        (error) => {
          // Handle timeout error
          console.error('Request timed out:', error);
        }
      );
  }
}
