import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListBookingsComponent } from './list-tasks.component';

describe('ListBookingsComponent', () => {
  let component: ListBookingsComponent;
  let fixture: ComponentFixture<ListBookingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListBookingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListBookingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
