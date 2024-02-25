import { Component, OnInit } from '@angular/core';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  isUserAuthenticated = false;
  userName = 'Balázs';

  constructor(private user: UserService) {}
  ngOnInit(): void {
    this.isUserAuthenticated = this.user.isUserLoggedIn;
  }

  onLog() {
    this.user.isUserLoggedIn = !this.user.isUserLoggedIn;
    this.isUserAuthenticated = this.user.isUserLoggedIn;
  }
}
