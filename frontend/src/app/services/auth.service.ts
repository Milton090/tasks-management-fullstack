import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environment/environment';
import { Auth, ResponseAPI } from '../interfaces';
import { Observable, tap } from 'rxjs';
import { AlertService } from './alert.service';


@Injectable({
  providedIn: 'root'
})

export class AuthService {

  private url = environment.apiUrl + environment.authController;

  constructor(private http: HttpClient, private alert: AlertService) { }

  login(user: Auth): Observable<ResponseAPI> {
    return this.http.post<ResponseAPI>(`${this.url}/login`, user).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Sesion iniciada', 'Bienvenido al sistema', 'token'))
    );
  }

  register(user: Auth): Observable<ResponseAPI> {
    return this.http.post<ResponseAPI>(`${this.url}/register`, user).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Usuario registrado', 'Ahora debes iniciar sesion'))
    );
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  private handleSuccess(res: ResponseAPI, title: string, message: string, key?: string): void {
    if (res.success) {
      this.alert.success(title, message);
      if (key && res.data[key]) {
        localStorage.setItem(key, res.data[key]);
      }
    }
  }
}
