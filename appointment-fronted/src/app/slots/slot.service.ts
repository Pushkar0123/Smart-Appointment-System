import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
// import { Slot } from './slot.model';

@Injectable({
  providedIn: 'root'
})
export class SlotService {

  private baseUrl = 'http://localhost:5284/api/slots';

  constructor(private http: HttpClient) {}


  //  DOCTOR – ALL SLOTS
  getDoctorSlots() {
    return this.http.get<any[]>(`${this.baseUrl}/doctor`);
  }

  // PATIENT – AVAILABLE SLOTS
  getAvailableSlots(page: number = 1, pageSize: number = 5) {
    return this.http.get<any>(
      `${this.baseUrl}/available?page=${page}&pageSize=${pageSize}`
    );
  }


  //  CREATE SLOT (Doctor)
  createSlot(data: { startTime: string; endTime: string }) {
    return this.http.post(this.baseUrl, data);
  }

  //  UPDATE SLOT (Doctor)
  updateSlot(id: number, data: any) {
    return this.http.put(`${this.baseUrl}/${id}`, data);
  }

  // DELETE SLOT (Doctor)
  deleteSlot(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  // getExpiredSlots() {
  //   return this.http.get<Slot[]>(
  //     `${this.baseUrl}/expired`
  //   );
  // }

  // deleteExpiredSlot(id: number) {
  //   return this.http.delete(
  //     `${this.baseUrl}/expired/${id}`
  //   );
  // }

  deleteExpiredSlot(id: number) {
    return this.http.delete(
      // `http://localhost:5284/api/slots/expired/${id}`
        `${this.baseUrl}/expired/${id}`
    );
  }


}
