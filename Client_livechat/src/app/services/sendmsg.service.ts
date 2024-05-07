import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class SendmsgService {
  apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) {}

  sendMessage(msgId: string, message: string) {
    const url = `${this.apiUrl}Message/sendMessage/${msgId}`;

    // Headers
    const headers = new HttpHeaders({

    });

    // Body
    const body = JSON.stringify({
      text: message,
    });
    let tokenKey = localStorage.getItem('token');

    let headerCustom = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${tokenKey}`
    });
    // Effettua la richiesta POST
    return this.http.post(url, body, { headers: headerCustom });
  }
}
