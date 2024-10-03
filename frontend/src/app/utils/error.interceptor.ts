import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AlertService } from '../services/alert.service';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const alert = inject(AlertService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = '';

      if (error.status == 401){
        alert.warning('Sesión caducada', 'Su sesión ha caducado, por favor inicie sesión nuevamente');
        router.navigate(['/login']);
        return throwError(() => new Error('Sesión caducada'));
      }
      
      if (error.error instanceof ErrorEvent) {
        errorMessage = `${error.message}`;
      } else {
        errorMessage =  error.error.message || `${error.status} - ${error.message}`;
      }
      
      alert.error(errorMessage);
      return throwError(() => new Error(errorMessage));
    })
  );
};
