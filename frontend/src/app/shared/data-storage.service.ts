import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class DataStorageService {
  private backendUrl: string = environment.backendUrl;

  constructor(private http: HttpClient) {}

  getRooms() {
    return this.http.get(this.backendUrl + '/rooms');
  }
}
