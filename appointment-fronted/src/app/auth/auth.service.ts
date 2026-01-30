import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private apiUrl = 'http://localhost:5284/api/auth';

  private isBrowser: boolean;
  
  private authReadySubject = new BehaviorSubject<boolean>(false);
  authReady$ = this.authReadySubject.asObservable();


  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);

    if (this.isBrowser) {
      const token = localStorage.getItem('token');
      this.authReadySubject.next(!!token);
    }
  }

  // LOGIN API CALL
  login(username: string, password: string) {
    return this.http.post(`${this.apiUrl}/login`, {
      username,
      password
    });
  }

  // SAVE TOKEN
  saveToken(token: string) {
    if (this.isBrowser) {
      localStorage.setItem('token', token);
      this.authReadySubject.next(true);
    }
  }

  // GET TOKEN
  getToken(): string | null {
    return this.isBrowser ? localStorage.getItem('token') : null;
  }

  // ROLE FROM JWT
  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout() {
    if (this.isBrowser) {
      localStorage.removeItem('token');
      this.authReadySubject.next(false);
    }
    this.authReadySubject.next(false);
    this.router.navigate(['/login']);
  }
}

