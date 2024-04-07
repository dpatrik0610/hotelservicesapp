import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Room } from './room.model';

@Injectable({
  providedIn: 'root',
})
export class DataStorageService {
  private backendUrl: string = environment.backendUrl;

  constructor(private http: HttpClient) {}

  getRooms() {
    return this.http.get<Room[]>(this.backendUrl + '/Room/available');
  }

  getRoom(id: number) {
    return this.http.get<Room>(this.backendUrl + '/Room/' + id);
  }
}
