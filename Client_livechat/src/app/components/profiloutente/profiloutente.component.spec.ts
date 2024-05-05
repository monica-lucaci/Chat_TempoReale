import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfiloutenteComponent } from './profiloutente.component';

describe('ProfiloutenteComponent', () => {
  let component: ProfiloutenteComponent;
  let fixture: ComponentFixture<ProfiloutenteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfiloutenteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProfiloutenteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
