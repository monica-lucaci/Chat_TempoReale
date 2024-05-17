import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatroomService } from '../../services/chatroom.service';
import { Risposta } from '../../interfaces/risposta';
import { Chatroom } from '../../models/chatroom';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-createchat',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './createchat.component.html',
  styleUrl: './createchat.component.css'
})
export class CreatechatComponent {

  chatroomCrd ="";
  @Input('show')
  show=true;

  @Output('close')
  onClose = new EventEmitter()

  @Output() chatroomCreated = new EventEmitter();

  @Input() currentUser: string | null = null;; // Define currentUser as an input property
  @Input() selectedChatroom:  Chatroom | null = null;
  newChatroom: Chatroom = new Chatroom(); 
 
  users: User[] = [];

  constructor(private chatroomService: ChatroomService, private userService: UserService,private snackBar: MatSnackBar) {
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


          if (response.data && response.data.crCd) {
            this.chatroomCrd = response.data.crCd;
            console.log(this.chatroomCrd);
            this.chatroomCreated.emit(this.chatroomCrd);
          }

        //  this.onClose.emit();
        this.openSnackBar('ChatRoom created successfully');
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

  addToChatroom(user: User, chatroomCode: string) {
   
    if (chatroomCode && user.user) {
      this.chatroomService.addUserToChatRoom(chatroomCode, user.user).subscribe(
        (response: Risposta) => {
          // Handle success
          console.log('User added to chat room successfully:', response);
          
          this.openSnackBar('User added successfully');
          // Optionally, you can update the UI or take further actions upon success
        },
        (error:any ) => {
          // Handle error
          console.error('Error adding user to chat room:', error);
          this.openSnackBar('Error adding user to chat room');
        }
      );
    } else {
      console.error('Invalid data to add user to chat room');
      this.openSnackBar('Invalid data');
    }
  }
  openSnackBar(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000, // Duration in milliseconds
      horizontalPosition: 'center', // Position horizontally (start, center, end, left, right)
      verticalPosition: 'bottom', // Position vertically (top, bottom)
    });
  
  
}
}