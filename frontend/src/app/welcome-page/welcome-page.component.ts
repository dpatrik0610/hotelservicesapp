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
    // this.getRoomsSubscription = this.dataStorage
    //   .getRooms()
    //   .subscribe((data) => {
    //     console.log(data);
    //   });
  }
}
