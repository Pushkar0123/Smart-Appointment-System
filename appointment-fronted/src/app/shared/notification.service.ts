import { Injectable } from '@angular/core';


export class NotificationService {

  sendEmail(data: any) {
    return {
      to: data.username + '@example.com',
      subject: 'Appointment Confirmation',
      body: `
Hello ${data.username},

Your appointment is confirmed.

Booking ID: ${data.bookingId}
Slot ID: ${data.slotId}

Thank you.
`
    };
  }

  sendSMS(data: any) {
    return {
      to: '+91-XXXXXXXXXX',
      message: `Your booking (ID: ${data.bookingId}) is confirmed.`
    };
  }
}

