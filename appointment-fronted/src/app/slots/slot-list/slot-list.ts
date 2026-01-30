import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { finalize,filter } from 'rxjs/operators';
import { SlotService } from '../slot.service';
import { BookingService } from '../../bookings/booking.service';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-slot-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './slot-list.html',
  styleUrls: ['./slot-list.css']
})
export class SlotListComponent implements OnInit {

  slots: any[] = [];
  loading = false;

  userRole : string | null = null;

  constructor(
    private slotService: SlotService,
    private bookingService: BookingService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    // if (this.authService.isLoggedIn()) {
    // this.loadSlots();
    this.userRole = this.authService.getUserRole();

    setTimeout(() => {
      this.loadSlots();
    }, 0);
    }

  loadSlots() {
    console.log('loadSlots() CALLED');

    this.loading = true;

    this.slotService.getAvailableSlots()
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cdr.detectChanges(); //  ensure UI update
          console.log('loading finished');
        })
      )
      .subscribe({
        next: (res: any) => {
          console.log('✅ Slots received:', res);

          // Normalize backend response
          const rawSlots = res?.data ?? res ?? [];

          this.slots = rawSlots.map((s: any) => ({
            ...s,
            status: s.status ?? s.Status   //backend casing issue
          }));
          this.cdr.detectChanges(); // ensure UI update
        },
        error: (err) => {
          console.error('Slot error:', err);
          this.slots = [];
          this.cdr.detectChanges(); //ensure UI update
        }
      });
  }

  book(slotId: number) {
    this.bookingService.bookSlot(slotId).subscribe({
      next: (res: any) => {
        // console.log('Raw API booking response:', res);
        // this.slots = res.data ?? []; // update slots list
        this.router.navigate(['/booking-confirmation'], {
          state: {
            bookingId: res.bookingId,
            slotId
          }
        });

        // refresh slots after booking
        this.loadSlots();
      },
      error: err => {
        console.error('Booking error:', err);
        alert(err.error?.message || err.error ||'Booking failed');
      }
    });
  }

  getStatusClass(status: string) {
    return {
      available: status === 'Available',
      booked: status === 'Booked',
      expired: status === 'Expired'
    };
  }
}

