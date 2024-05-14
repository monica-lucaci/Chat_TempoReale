import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddusertochatComponent } from './addusertochat.component';

describe('AddusertochatComponent', () => {
  let component: AddusertochatComponent;
  let fixture: ComponentFixture<AddusertochatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddusertochatComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddusertochatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
