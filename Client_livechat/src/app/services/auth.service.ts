import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable, throwError } from 'rxjs';
import { LoginRequest } from '../interfaces/login-request';
import { AuthResponse } from '../interfaces/auth-response';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { Risposta } from '../interfaces/risposta';
import { User } from '../models/user';
import { UserService } from './user.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  apiUrl: string = environment.apiUrl;
  private tokenKey = 'token';
 

  constructor(
    private http: HttpClient,
    private userService: UserService,
    private router: Router,
  ) {}

  login(email: string, user: string, pass: string): Observable<string> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    const payload = { email, user, pass };

    return this.http.post<string>(`${this.apiUrl}Auth/login`, payload, {
      headers: headers,
      responseType: 'text' as 'json', // Tells Angular to handle the response as plain text
    });
  }

  private getToken = (): string | null => localStorage.getItem(this.tokenKey);

  getCurrentUser(): string | null {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        console.log(decodedToken.Username);
        return decodedToken.Username; // Assuming 'Username' is the property that stores the username in your JWT payload
      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
    }
    return null;
  }

  isLoggedIn = (): boolean => {
    const token = this.getToken();
    if (!token) return false;

    return !this.isTokenExpired();
  };

  isTokenExpired() {
    const token = this.getToken();
    if (!token) return true;
    const decoded = jwtDecode(token);
    const isTokenExpired = Date.now() >= decoded['exp']! * 1000;
   // console.log("here you see when token expires " + isTokenExpired)
    if (isTokenExpired) this.logout();
   // console.log('is token expired ' + Date.now() + " " + decoded['exp']! * 1000);
    return isTokenExpired;
  }

  logout = (): void => {
    localStorage.removeItem(this.tokenKey);
    this.router.navigateByUrl('/login');
  };

  registra(
    email: string,
    user: string,
    pass: string,
    img: string = '',
  ): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = JSON.stringify({ email, user, pass, img });

    // Include the query parameter for 'img'
    return this.http
      .post(`${this.apiUrl}User/register?img=${img}`, body, { headers })
      .pipe(
        catchError((error) => {
          console.error('Registration error:', error);
          return throwError(
            () => new Error('Registration failed due to server error'),
          );
        }),
      );
  }
}
