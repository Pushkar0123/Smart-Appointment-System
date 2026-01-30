import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'http://localhost:5284/api/bookings';  // Adjust endpoint if needed

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  bookSlot(slotId: number) {
    return this.http.post<any>(
      `http://localhost:5284/api/bookings/${slotId}`,
      {}
    );
  }
}
