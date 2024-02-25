import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private _isUserLoggedIn: boolean = false; // This variable will be used to check if the user is logged in or not

  public get isUserLoggedIn(): boolean {
    return this._isUserLoggedIn;
  }
  public set isUserLoggedIn(value: boolean) {
    this._isUserLoggedIn = value;
  }

  constructor() {}
}
