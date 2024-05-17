import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Risposta } from '../../interfaces/risposta';
import { User } from '../../models/user';
import { ChatroomService } from '../../services/chatroom.service';
import { UserService } from '../../services/user.service';
import { Chatroom } from '../../models/chatroom';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-addusertochat',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './addusertochat.component.html',
  styleUrl: './addusertochat.component.css'
})
export class AddusertochatComponent {
  @Input('show2')
  show2=true;

  @Output('close')
  onClose = new EventEmitter()
  @Input() currentUser: string | null = null; // Define currentUser as an input property
  @Input() selectedChatroom:  Chatroom | null = null;
  newChatroom: Chatroom = new Chatroom(); 
  users: User[] = [];

  constructor(private chatroomService: ChatroomService, private userService: UserService,private snackBar: MatSnackBar) {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe(
      (response: Risposta) => {
        // Filter out the current user
        this.users = response.data.filter((user: User) => user.user !== this.currentUser);
        console.log(this.users);
      },
      (error: any) => {
        console.error('Error fetching users:', error);
      }
    );
  }
  




  addToChatroom(user: User, selectedChatroom: Chatroom | null) {
    const crCode = this.selectedChatroom?.crCd;
    console.log(this.selectedChatroom?.crCd);
    if (crCode && user.user) {
      this.chatroomService.addUserToChatRoom(crCode, user.user).subscribe(
        (response: Risposta) => {
          // Handle success
          console.log('User added to chat room successfully:', response);
           this.loadUsers();
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
      duration: 3000 // 3 seconds
    });
  }
  
  
  
}


