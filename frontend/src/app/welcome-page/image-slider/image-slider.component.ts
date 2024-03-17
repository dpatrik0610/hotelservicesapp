import { Component, Input } from '@angular/core';

export interface Slide {
  imageSrc: string;
  imageAlt: string;
}

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrl: './image-slider.component.scss',
})
export class ImageSliderComponent {
  @Input() images: Slide[] = [];

  selectedIndex = 0;

  showPrev(i: number) {
    if (this.selectedIndex > 0) {
      this.selectedIndex = i - 1;
    }
  }

  showNext(i: number) {
    if (this.selectedIndex < this.images.length - 1) {
      this.selectedIndex = i + 1;
    }
  }
}
