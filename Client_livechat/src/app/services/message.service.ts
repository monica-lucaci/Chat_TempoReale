import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Risposta } from '../interfaces/risposta';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl: string = environment.apiUrl;

  constructor(private http:HttpClient) { }

  deleteMessage(messageId:string){
    let tokenKey = localStorage.getItem('token');

    let headerCustom = new HttpHeaders({
      Authorization: `Bearer ${tokenKey}`,
    });



    //da implementare
    return this.http.delete<Risposta>(`${this.apiUrl}deleteMessage`, {
      headers: headerCustom,
    });
  }
}
