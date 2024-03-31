import { Component, OnInit } from '@angular/core';
import { DataStorageService } from '../shared/data-storage.service';
import { Subscription, timeout } from 'rxjs';
import { Slide } from './image-slider/image-slider.component';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.component.html',
  styleUrl: './welcome-page.component.css',
})
export class WelcomePageComponent implements OnInit {
  getRoomsSubscription = new Subscription();

  // Placeholder images for the image slider
  images: Slide[] = [
    { imageSrc: '../../assets/img/photo-1.jpg', imageAlt: 'Room 1' },
    { imageSrc: '../../assets/img/photo-2.jpg', imageAlt: 'Room 2' },
    { imageSrc: '../../assets/img/luxury-bg.jpg', imageAlt: 'Room 3' },
    { imageSrc: '../../assets/img/person-1.jpg', imageAlt: 'Room 4' },
  ];

  constructor(private dataStorage: DataStorageService) {}

  ngOnInit() {
    // this.getRoomsSubscription = this.dataStorage
    //   .getRooms()
    //   .subscribe((data) => {
    //     console.log(data);
    //   });
  }
}
