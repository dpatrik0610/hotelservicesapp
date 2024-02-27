import { Component, Host, HostListener, OnInit } from '@angular/core';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  isUserAuthenticated = false;
  userName = 'Balázs';
  navbarFixed = false;

  @HostListener('window:scroll', ['$event']) onScroll() {
    if (window.scrollY > 100) {
      this.navbarFixed = true;
    } else {
      this.navbarFixed = false;
    }
  }

  constructor(private user: UserService) {}
  ngOnInit(): void {
    this.isUserAuthenticated = this.user.isUserLoggedIn;
  }

  onLog() {
    this.user.isUserLoggedIn = !this.user.isUserLoggedIn;
    this.isUserAuthenticated = this.user.isUserLoggedIn;
  }
}
