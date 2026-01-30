import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SlotService } from '../../slots/slot.service';

@Component({
  // selector: 'app-doctor-slot-manager',
  selector: 'app-createslot',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor-slot-manager.html',
  styleUrls: ['./doctor-slot-manager.css']
})
export class DoctorSlotManagerComponent implements OnInit {

  slots: any[] = [];

  startTime = '';
  endTime = '';
  loading = false;

  constructor(private slotService: SlotService) {}

  ngOnInit() {
    this.loadSlots();
  }

  loadSlots() {
    this.slotService.getDoctorSlots().subscribe((res : any) => {
      this.slots = res.data ?? res;
    });
  }

  createSlot() {
    if (!this.startTime || !this.endTime) {
      alert('Start & End time required');
      return;
    }

    this.slotService.createSlot({
      startTime: this.startTime,
      endTime: this.endTime
    }).subscribe(() => {
      this.startTime = '';
      this.endTime = '';
      this.loadSlots();
    });
  }

  deleteSlot(id: number) {
    if (!confirm('Delete this slot?')) return;

    this.slotService.deleteSlot(id).subscribe(() => {
      this.loadSlots();
    });
  }
}

