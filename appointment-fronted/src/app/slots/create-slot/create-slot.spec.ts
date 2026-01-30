import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSlotComponent } from './create-slot';

describe('CreateSlot', () => {
  let component: CreateSlotComponent;
  let fixture: ComponentFixture<CreateSlotComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateSlotComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateSlotComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
