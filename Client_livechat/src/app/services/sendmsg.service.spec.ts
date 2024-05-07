import { TestBed } from '@angular/core/testing';

import { SendmsgService } from './sendmsg.service';

describe('SendmsgService', () => {
  let service: SendmsgService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SendmsgService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
