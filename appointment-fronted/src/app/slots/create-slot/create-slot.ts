import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SlotService } from '../slot.service';

@Component({
  selector: 'app-create-slot',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-slot.html',
  styleUrls: ['./create-slot.css']
})
export class CreateSlotComponent {

  startTime = '';
  endTime = '';
  loading = false;
  error = '';

  @Output() slotCreated = new EventEmitter<void>();

  constructor(private slotService: SlotService) {}

  createSlot() {
    if (!this.startTime || !this.endTime) {
      this.error = 'Start time and End time are required';
      return;
    }

    // Convert to ISO format (MAIN FIX)
    const payload = {
      startTime: new Date(this.startTime).toISOString(),
      endTime: new Date(this.endTime).toISOString()
    };

    this.loading = true;
    this.error = '';

    this.slotService.createSlot(payload).subscribe({
      next: () => {
        this.loading = false;

        this.slotCreated.emit(); // refresh dashboard

        // Clear form
        this.startTime = '';
        this.endTime = '';
      },
      error: err => {
        this.error = err.error || 'Failed to create slot';
        this.loading = false;
      }
    });
  }
}
