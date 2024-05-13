import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Risposta } from '../interfaces/risposta';
import { environment } from '../../environments/environment.development';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl: string = environment.apiUrl;

  constructor(private http:HttpClient) { }

  sendMessage(message: any, crCode: string, sder: string): Observable<Risposta> {
    // Include sender in the message object
    const messageWithSender = { ...message, sder: sder };
    
    return this.http.post<Risposta>(`${this.apiUrl}Message/sendMessage/${crCode}`, messageWithSender)
      .pipe(
        catchError(this.handleError)
      );
  }
  

  getMessagesOfRoom(crCode: string): Observable<Risposta> {
    return this.http.get<Risposta>(`${this.apiUrl}Message/MessagesOfARoom/${crCode}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteMessage(messageCode: string, username: string): Observable<Risposta> {
    return this.http.delete<Risposta>(`${this.apiUrl}Message/chat/deleteMessage/${messageCode}?username=${username}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteMessages(username: string): Observable<Risposta> {
    return this.http.delete<Risposta>(`${this.apiUrl}Message/chat/deleteMessagesByUser/${username}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  updateMessage(messageCode: string, username: string, textMessage: string): Observable<Risposta> {
    return this.http.post<Risposta>(`${this.apiUrl}Message/UpdateMessage/${messageCode}?username=${username}&textMessage=${textMessage}`, {})
      .pipe(
        catchError(this.handleError)
      );
  }
  private handleError(error: any): Observable<any> {
    console.error('An error occurred:', error);
    return throwError(error);
  }
}

