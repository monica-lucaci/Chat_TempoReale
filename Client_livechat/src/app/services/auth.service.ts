import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { LoginRequest } from '../interfaces/login-request';
import { AuthResponse } from '../interfaces/auth-response';
import { HttpClient , HttpHeaders} from '@angular/common/http';
import { map } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { Risposta } from '../interfaces/risposta';
import { User } from '../models/user';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  apiUrl: string = environment.apiUrl;
  private tokenKey='token';

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<AuthResponse> {
    // return this.http.post<any>(`${this.apiUrl}Auth/login`, { username, password })
    //   .pipe(
    //     map((response: any) => {
    //       if (response.status === 'SUCCESS') {
    //         localStorage.setItem(this.tokenKey, response.data.token);
    //       }
    //       return response;
    //     })
    //   );
    //}
    let headerCustom = new HttpHeaders();
    headerCustom.set('Content-Type', 'application/json');

    let invio = {
      username,
      password,
    };

    return this.http.post<any>(`${this.apiUrl}Auth/login`, invio, {
      headers: headerCustom,
    });
  }
  


  getUserDetail(): any {
    const token = this.getToken();
    if (!token) return null;
    const decodedToken: any = jwtDecode(token);
    const userDetail = {
      username: decodedToken.Username,
      // Add more fields as needed
    };
    return userDetail;
  }



  isLoggedIn = (): boolean=>{
    const token = this.getToken();
    if(!token) return false;

    return !this.isTokenExpired();
  }

  private isTokenExpired() {
   const token = this.getToken();
   if(!token) return true;
   const decoded = jwtDecode(token);
   const isTokenExpired = Date.now() >= decoded['exp']! * 1000;
   if(isTokenExpired) this.logout();
   return isTokenExpired;
  }

  logout = (): void => {
    localStorage.removeItem(this.tokenKey)
  }

  registra(user: string, pass: string): Observable<Risposta> {
    let utente: User = new User();
    utente.user = user;
    utente.pass = pass;
    return this.http.post<any>(`${this.apiUrl}user/register`, utente);
  }



  private getToken = () : string | null => localStorage.getItem(this.tokenKey )

}
