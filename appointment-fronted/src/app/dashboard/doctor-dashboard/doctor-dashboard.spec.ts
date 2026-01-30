import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorDashboardComponent } from './doctor-dashboard';

describe('DoctorDashboard', () => {
  let component: DoctorDashboardComponent;
  let fixture: ComponentFixture<DoctorDashboardComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorDashboardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
