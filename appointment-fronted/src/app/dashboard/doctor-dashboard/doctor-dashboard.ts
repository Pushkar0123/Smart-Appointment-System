import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../auth/auth.service';
import { SlotService } from '../../slots/slot.service';
import { Router } from '@angular/router';
import { filter, finalize } from 'rxjs';
import { CreateSlotComponent } from '../../slots/create-slot/create-slot';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    CreateSlotComponent
  ],
  templateUrl: './doctor-dashboard.html',
  styleUrls: ['./doctor-dashboard.css']
})
export class DoctorDashboardComponent implements OnInit {

  slots: any[] = [];
  loading = true;
  showCreate = false;

  constructor(
    private authService: AuthService,
    private slotService: SlotService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  // Wait for JWT token → then load slots
  ngOnInit() {
    this.authService.authReady$
      .pipe(filter(ready => ready === true))
      .subscribe(() => {
        this.loadSlots();
        // this.loadExpiredSlots();
      });
  }

  // Load slots (single source of truth)
  loadSlots() {
    this.loading = true;

    this.slotService.getDoctorSlots()
      .pipe(finalize(() => {
        this.loading = false;
        this.cdr.detectChanges(); // Ensure UI updates
      }))
      .subscribe({
        next: (res: any) => {
          // Doctor API returns ARRAY, not { data }
          this.slots = Array.isArray(res) ? res : [];
          // this.cdr.detectChanges();
        },
        error: () => {
          this.slots = [];
          // this.cdr.detectChanges();
        }
      });
  }

  // ➕ Toggle Create Slot form
  toggleCreate() {
    this.showCreate = !this.showCreate;
  }

  // 🔔 Called after slot creation
  onSlotCreated() {
    this.showCreate = false;
    this.loadSlots();
    console.log('Slot created, refresh slot list');
  }

  // 🗑 Delete slot (only Available)
  deleteSlot(id: number) {
    if (!confirm('Delete this slot?')) return;

    this.slotService.deleteSlot(id).subscribe(() => {
      this.loadSlots();
    });
  }

  

  // // Delete EXPIRED slot
      deleteExpiredSlot(id: number) {
      if (!confirm('Delete expired slot?')) return;

      this.slotService.deleteExpiredSlot(id).subscribe({
        next: () => {
          this.loadSlots(); // refresh dashboard
        },
        error: err => {
          alert('Slot deleted');
          this.loadSlots();
        }
      });
    }



  // deleteExpiredSlot(id: number) {
  //     if (!confirm('Are you sure you want to delete this expired slot?')) {
  //       return;
  //     }

  //     this.slotService.deleteExpiredSlot(id).subscribe({
  //       next: () => {
  //         alert('Expired slot deleted');
  //         this.loadExpiredSlots(); // refresh expired list
  //       },
  //       error: err => {
  //         alert(err.error);
  //       }
  //     });
  //   }


  // 🎨 Status UI helper
  getStatusClass(status: string) {
    return {
      available: status === 'Available',
      booked: status === 'Booked',
      expired: status === 'Expired'
    };
  }

  // expiredSlots: any[] = [];

  // loadExpiredSlots() {
  //   this.slotService.getExpiredSlots().subscribe({
  //     next: (res: any) => {
  //       this.expiredSlots = Array.isArray(res) ? res : [];
  //       this.cdr.detectChanges();
  //     },
  //     error: () => {
  //       this.expiredSlots = [];
  //     }
  //   });
  // }


  // 🚪 Logout
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}