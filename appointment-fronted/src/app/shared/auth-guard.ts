// import { HttpInterceptorFn } from '@angular/common/http';
// import { inject } from '@angular/core';
// import { AuthService } from '../auth/auth.service';

// export const authInterceptor: HttpInterceptorFn = (req, next) => {
//   const auth = inject(AuthService);
//   const token = auth.getToken();

//   if (token) {
//     req = req.clone({
//       setHeaders: {
//         Authorization: `Bearer ${token}`
//       }
//     });
//   }

//   return next(req);
// };


import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (!auth.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  return true;
};
