import { Routes } from '@angular/router';
import { authGuard } from './shared/auth-guard';
import { PatientDashboardComponent } from './dashboard/patient-dashboard/patient-dashboard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./auth/login/login')
        .then(m => m.LoginComponent)
  },

  // PATIENT SLOTS PAGE
  {
    path: 'slots',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./slots/slot-list/slot-list')
        .then(m => m.SlotListComponent)
  },

  // DOCTOR DASHBOARD
  {
    path: 'doctor',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./dashboard/doctor-dashboard/doctor-dashboard')
        .then(m => m.DoctorDashboardComponent)
  },

  // PATIENT DASHBOARD (optional)
  {
    path: 'patient',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./dashboard/patient-dashboard/patient-dashboard')
        .then(m => m.PatientDashboardComponent)
  },

  {
  path: 'patient',
  component: PatientDashboardComponent
  },


  // BOOKING CONFIRMATION
  {
    path: 'booking-confirmation',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./bookings/booking-confirmation/booking-confirmation')
        .then(m => m.BookingConfirmation)
  },

  // DEFAULT
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  // FALLBACK SAFETY
  {
    path: '**',
    redirectTo: 'login'
  }
];



