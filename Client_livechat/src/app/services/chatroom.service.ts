import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserService } from './user.service';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { Risposta } from '../interfaces/risposta';
import { Chatroom } from '../models/chatroom';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ChatroomService {
  apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient, private UtentiSvc:UserService,private authService: AuthService ) {
  }

  getChatsForCurrentUser(): Observable<Risposta> {
    const currentUser = this.authService.getCurrentUser();
    return this.http.get<Risposta>(`${this.apiUrl}chats/${currentUser}`);
  }


//-----------------------------------------------------
createChatroom(newRoom: Chatroom, user: string): Observable<Risposta> {
  return this.http.post<Risposta>(`${this.apiUrl}ChatRoom/newChatRoom/${user}`, newRoom);
}

getChatrooms(): Observable<Risposta> {
  return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/chat/viewList`);
}

getChatroomsOfUser(username: string): Observable<Risposta> {
  return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/viewChatRooms/${username}`);
}

getChatroomAndMessages(cr_code: string): Observable<Risposta> {
  return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/chat/${cr_code}`);
}

addUserToChatRoom(cr_code: string, username: string): Observable<Risposta> {
  return this.http.post<Risposta>(`${this.apiUrl}ChatRoom/chat/addUser/${cr_code}`, { username });
}

deleteUserFromChatRoom(cr_code: string, username: string): Observable<Risposta> {
  return this.http.delete<Risposta>(`${this.apiUrl}ChatRoom/chat/removeUser/${cr_code}/${username}`);
}

getUsersByRoom(cr_code: string): Observable<Risposta> {
  return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/usersOfRoom/${cr_code}`);
}

deleteChatRoom(cr_code: string, username: string): Observable<Risposta> {
  return this.http.delete<Risposta>(`${this.apiUrl}ChatRoom/chat/deleteChatRoom/${cr_code}/${username}`);
}

clearRoom(cr_code: string): Observable<Risposta> {
  return this.http.delete<Risposta>(`${this.apiUrl}ChatRoom/chat/ClearRoom/${cr_code}`);
}
}


//---------------------------------------
//   getRoom(crId: string): Observable<Risposta> {

//     return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/chat/${crId}`, {

//     });
//   }
//   getRooms(): Observable<any> {
//     return this.http.get<any>(`${this.apiUrl}ChatRoom/chat/viewList"`);
//   }


//   newChatRoom(crTitle: string, crDesc: string, user: string): Observable<Response> {
//     let chat:Chatroom = new Chatroom();
//     chat.titl = crTitle;
//     chat.desc = crDesc;

//     let tokenKey = localStorage.getItem('token');

//     let headersCustom = new HttpHeaders({
//       'Content-Type': 'application/json',
//       Authorization: `Bearer ${tokenKey}`
//     });

//     return this.http.post<Response>(`${this.apiUrl}ChatRoom/newChatRoom/${user}`, chat, { headers: headersCustom });
//   }

//   AddUser(id:string, username:string):Observable<Risposta>{
//     return this.http.post<Risposta>(`${this.apiUrl}ChatRoom/chat/addUser/${id}?username=${username}`, username)
//   }

//  getUserByRoom(id:string):Observable<Risposta>{
//     return this.http.get<Risposta>(`${this.apiUrl}ChatRoom/userOfRoom/${id}`)
//   }
