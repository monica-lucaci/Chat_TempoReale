import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatroomService } from '../../services/chatroom.service';
import { Risposta } from '../../interfaces/risposta';
import { Chatroom } from '../../models/chatroom';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-createchat',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './createchat.component.html',
  styleUrl: './createchat.component.css'
})
export class CreatechatComponent {


  @Input('show')
  show=true;

  @Output('close')
  onClose = new EventEmitter()

  @Input() currentUser: string | null = null;; // Define currentUser as an input property
  newChatroom: Chatroom = new Chatroom(); 
  users: User[] = [];

  constructor(private chatroomService: ChatroomService, private userService: UserService) {
    this.loadUsers();
  }
  loadUsers() {
    this.userService.getAllUsers().subscribe(
      (response: Risposta) => {
        this.users = response.data;
        console.log(this.users)
      },
      (error:any) => {
        console.error('Error fetching users:', error);
      }
    );
  }


  createChatroom() {
    if (this.newChatroom.titl && this.newChatroom.desc && this.currentUser) {
      // Set additional properties if needed


      this.chatroomService.createChatroomForCurrentUser(this.newChatroom, this.currentUser).subscribe(
        (response: Risposta) => {
          // Handle success
          console.log('Chat room created successfully:', response);
          console.log(response)
          this.newChatroom = new Chatroom(); // Reset the newChatroom object
          this.onClose.emit();
        },
        (error) => {
          // Handle error
          console.error('Error creating chat room:', error);
        }
      );
    } else {
      console.error('Invalid chat room data');
    }
  }

  addToChatroom(user: User) {
    console.log(this.newChatroom.crCd);
    console.log(user.user);
    if (this.newChatroom.crCd && user.user) {
    
      console.log(this.newChatroom.crCd);
      console.log(user.user);
      this.chatroomService.addUserToChatRoom(this.newChatroom.crCd, user.user).subscribe(
        (response: Risposta) => {
          // Handle success
          console.log('User added to chat room successfully:', response);
          // Optionally, you can update the UI or take further actions upon success
        },
        (error) => {
          // Handle error
          console.error('Error adding user to chat room:', error);
        }
      );
    } else {
      console.error('Invalid data to add user to chat room');
    }
  }
  
  
}
