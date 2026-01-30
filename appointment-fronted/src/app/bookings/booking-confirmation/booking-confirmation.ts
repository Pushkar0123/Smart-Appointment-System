import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationService } from '../../shared/notification.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-booking-confirmation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './booking-confirmation.html',
  styleUrls: ['./booking-confirmation.css']
})
export class BookingConfirmation {
  // bookingId: number | null = null;
  // slotId: number | null = null;
  bookingId!: number;
  slotId!: number;

  constructor(private router: Router) {
    const nav = this.router.currentNavigation();
    const state = nav?.extras.state as any;

    this.bookingId = state?.bookingId;
    this.slotId = state?.slotId;
  }

  goBack() {
    this.router.navigate(['/patient']);
  }
}
  
