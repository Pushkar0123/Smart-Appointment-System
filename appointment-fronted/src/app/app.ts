import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar';
import { PatientDashboardComponent } from './dashboard/patient-dashboard/patient-dashboard';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  template: `
    <app-navbar></app-navbar>
    <router-outlet></router-outlet>
  `,
  styleUrls: ['./app.css']
})

export class App {
  protected readonly title = 'appointment-fronted';
}
