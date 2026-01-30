import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorSlotManager } from './doctor-slot-manager';

describe('DoctorSlotManager', () => {
  let component: DoctorSlotManager;
  let fixture: ComponentFixture<DoctorSlotManager>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorSlotManager]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorSlotManager);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
