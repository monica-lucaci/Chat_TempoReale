import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { LoginRequest } from '../interfaces/login-request';
import { AuthResponse } from '../interfaces/auth-response';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  apiUrl: string = environment.apiUrl;
  private tokenKey='token';

  constructor(private http: HttpClient) { }

  login(data : LoginRequest) : Observable<AuthResponse>{
    return this.http
    .post<AuthResponse>(`${this.apiUrl}Auth/login`,data)
    .pipe(
      map((response) => {
        if(response.isSuccess){
          localStorage.setItem(this.tokenKey,response.token)
        }
        return response;
      })
    )
  }

  getUserDetail= () => {
    const token = this.getToken();
    if(!token) return true;
    const decodedToken:any = jwtDecode(token);
    const userDetail = {
      username: decodedToken.username,
      passwrd: decodedToken.passwrd,
      usImg: decodedToken.usImg,
      isDeleted: decodedToken.isDeleted,

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

  private getToken = () : string | null => localStorage.getItem(this.tokenKey )

}
