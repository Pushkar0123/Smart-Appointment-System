// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-login',
//   imports: [],
//   templateUrl: './login.html',
//   styleUrl: './login.css',
// })
// export class Login {

// }
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  username = '';
  password = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

//   login() {
//   console.log('LOGIN BUTTON CLICKED');
//   console.log('LOGIN DATA:', this.username, this.password);

//   this.authService.login(this.username, this.password).subscribe({
//     next: (res: any) => {
//       console.log('LOGIN RESPONSE:', res);
//       this.authService.saveToken(res.token);
//       this.router.navigate(['/slots']);
//     },
//     error: (err) => {
//       console.error('LOGIN ERROR:', err);
//     }
//   });
// }
  login() {
  console.log('LOGIN BUTTON CLICKED');
  console.log('LOGIN DATA:', this.username, this.password);

  this.authService.login(this.username, this.password).subscribe({
    next: (res: any) => {
      console.log('LOGIN RESPONSE:', res);

      // Save token
      this.authService.saveToken(res.token);

      // this.router.navigate(['/slots']);

      // ROLE-BASED REDIRECT
      const role = this.authService.getUserRole();

      if (role === 'Doctor') {
        this.router.navigate(['/doctor']);
      } else if (role === 'Patient') {
        this.router.navigate(['/patient']);
      } else if (role === 'Patient') {
        this.router.navigate(['/slots']);
      } else {
        // fallback safety
        this.router.navigate(['/login']);
      }
    },
    error: (err) => {
      console.error('LOGIN ERROR:', err);
      this.errorMessage = 'Invalid username or password';
    }
  });
}

}

// login() {
//   console.log('LOGIN BUTTON CLICKED');

//   this.authService.login(this.username, this.password).subscribe({
//     next: (res: any) => {
//       console.log('LOGIN RESPONSE:', res);

//       if (!res || !res.token) {
//         console.error('TOKEN NOT FOUND IN RESPONSE');
//         return;
//       }

//       this.authService.saveToken(res.token);
//       console.log('TOKEN SAVED:', localStorage.getItem('token'));

//       this.router.navigate(['/slots']);
//     },
//     error: (err) => {
//       console.error('LOGIN ERROR:', err);
//       this.errorMessage = 'Invalid username or password';
//     }
//   });
// }

