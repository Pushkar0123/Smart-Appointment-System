import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { filter } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css']
})
export class NavbarComponent {

  showNavbar = false;
  role: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    // Hide navbar on login page
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.showNavbar = !event.url.includes('/login');
        this.role = this.authService.getUserRole();
      });
  }

  logout() {
    this.authService.logout();
  }
}

