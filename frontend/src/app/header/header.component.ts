import { Component, Host, HostListener, OnInit } from '@angular/core';
import { UserService } from '../shared/user.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  isUserAuthenticated = false;
  userName = 'Balázs';
  navbarFixed = false;
  isLoginPage = false;

  @HostListener('window:scroll', ['$event']) onScroll() {
    if (window.scrollY > 100) {
      this.navbarFixed = true;
    } else {
      this.navbarFixed = false;
    }
  }

  constructor(private user: UserService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.isUserAuthenticated = this.user.isUserLoggedIn;

    this.route.url.subscribe((url) => {
      this.isLoginPage = url[0].path === 'login';
    });
  }

  onLog() {}
}
