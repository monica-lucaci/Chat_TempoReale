import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserService } from './user.service';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { Risposta } from '../interfaces/risposta';
import { Chatroom } from '../models/chatroom';

@Injectable({
  providedIn: 'root'
})
export class ChatroomService {

  apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient, private UtentiSvc:UserService ) {
  }

  getRoom(crId: string): Observable<Risposta> {

    return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/chat/${crId}`, {

    });
  }


  newChatRoom(crTitle: string, crDesc: string, user: string): Observable<Response> {
    let chat:Chatroom = new Chatroom();
    chat.titl = crTitle;
    chat.desc = crDesc;

    let tokenKey = localStorage.getItem('token');

    let headersCustom = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${tokenKey}`
    });

    return this.http.post<Response>(`${this.apiUrl}ChatRoom/newChatRoom/${user}`, chat, { headers: headersCustom });
  }

  AddUser(id:string, username:string):Observable<Risposta>{
    return this.http.post<Risposta>(`${this.apiUrl}ChatRoom/chat/addUser/${id}?username=${username}`, username)
  }

 getUserByRoom(id:string):Observable<Risposta>{
    return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/userOfRoom/${id}`)
  }
}
