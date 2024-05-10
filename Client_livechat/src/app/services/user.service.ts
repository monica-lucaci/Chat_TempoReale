import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Risposta } from '../interfaces/risposta';
import { User } from '../models/user';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getProfile(username: string = 'davide'): Observable<string> {
    let tokenKey = localStorage.getItem('token');

    let headerCustom = new HttpHeaders({
      Authorization: `Bearer ${tokenKey}`,
    });

    return this.http.get<string>(`${this.apiUrl}UserProfile/${username}`, {
      headers: headerCustom,
    });
  }

  getAllUsers(): Observable<Risposta> {
    return this.http.get<Risposta>(`${this.apiUrl}ListOfUsers`);
  }

  updateImg(user: User) {
    let tokenKey = localStorage.getItem('token');

    let headerCustom = new HttpHeaders({
      Authorization: `Bearer ${tokenKey}`,
    });

    return this.http.put<Risposta>(`${this.apiUrl}UpdateImage`, user, {
      headers: headerCustom,
    });
  }
}
