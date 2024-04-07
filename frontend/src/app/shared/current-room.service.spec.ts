import { TestBed } from '@angular/core/testing';

import { CurrentRoomService } from './current-room.service';

describe('CurrentRoomService', () => {
  let service: CurrentRoomService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CurrentRoomService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
