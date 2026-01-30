import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SlotService } from '../../slots/slot.service';
import { AuthService } from '../../auth/auth.service';
import { BookingService } from '../../bookings/booking.service';
import { Router } from '@angular/router'
import { Observable, map } from 'rxjs';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [CommonModule ],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboardComponent implements OnInit {

  // slots: any[] = [];
  // loading = true;
  slots$!: Observable<any[]>;


  constructor(
    private slotService: SlotService,
    private bookingService: BookingService,
    private authService: AuthService,
    // private cdr: ChangeDetectorRef
    private router: Router
  ) {}

  // ngOnInit() {
  //   this.loadAvailableSlots();
  // }

  ngOnInit() {
  this.slots$ = this.slotService.getAvailableSlots().pipe(
    map((res: any) =>
      (res?.data ?? []).map((s: any) => ({
        ...s,
        startTime: new Date(s.startTime),
        endTime: new Date(s.endTime)
      }))
    )
  );
}


//   loadAvailableSlots() {
//   this.loading = true;

//   this.slotService.getAvailableSlots()
//     .pipe(finalize(() => {
//         // this.loading = false;
//         // this.cdr.detectChanges(); // Ensure UI updates
//       }))
//     .subscribe({
//       next: (res) => {
//         console.log('Patient API response:', res);

//         // CORRECT NORMALIZATION
//         // this.slots = Array.isArray(res?.data)
//         //   ? res.data
//         //   : [];


//         // this.slots = [...(res?.data ?? [])];

//         this.slots = (res?.data ?? []).map((s: any) => ({
//           ...s,
//           startTime: new Date(s.startTime),
//           endTime: new Date(s.endTime)
//         }));


//       },
//       error: (err) => {
//         console.error('Slot load error', err);
//         this.slots = [];
//       }
//     });
// }


  // book(slotId: number) {
  //   this.bookingService.bookSlot(slotId).subscribe({
  //     next: () => {
  //       alert('Booking confirmed');
  //       this.loadAvailableSlots(); // refresh after booking
  //     },
  //     error: err => {
  //       alert(err.error || 'Booking failed');
  //     }
  //   });
  // }

  // book(slotId: number) {
  //   this.bookingService.bookSlot(slotId).subscribe({
  //     next: () => {
  //       alert('Booking confirmed');

  //       // re-fetch slots
  //       this.slots$ = this.slotService.getAvailableSlots().pipe(
  //         map((res: any) => res?.data ?? [])
  //       );
  //     },
  //     error: err => {
  //       alert(err.error || 'Booking failed');
  //     }
  //   });
  // }


  book(slotId: number) {
    this.bookingService.bookSlot(slotId).subscribe({
      next: (res) => {
        this.router.navigate(
          ['/booking-confirmation'],
          {
            state: {
              bookingId: res.bookingId,
              slotId: res.slotId
            }
          }
        );
      },
      error: err => {
        alert(err.error?.message || 'Booking failed');
      }
    });
  }


  logout() {
    this.authService.logout();
  }

  trackById(index: number, slot: any) {
    return slot.id;
  }
}