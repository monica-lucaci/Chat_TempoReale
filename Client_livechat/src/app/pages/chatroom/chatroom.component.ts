import { Component, OnInit } from '@angular/core';
import { Chatroom } from '../../models/chatroom';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { ChatroomService } from '../../services/chatroom.service';
import { SendmsgService } from '../../services/sendmsg.service';
import { SendmsgComponent } from '../../components/sendmsg/sendmsg.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chatroom',
  standalone: true,
  imports: [SendmsgComponent,CommonModule,FormsModule],
  templateUrl: './chatroom.component.html',
  styleUrl: './chatroom.component.css'
})
export class ChatroomComponent implements OnInit {
  chatrooms: Chatroom[] = [];
  selectedChatroom: Chatroom | null = null;
  currentUser: User | null = null;

  constructor(private chatroomService: ChatroomService, private userService: UserService, private sendmsgService: SendmsgService) {}

  ngOnInit(): void {
    this.fetchChatRooms();
    this.fetchCurrentUser();
  }

  fetchChatRooms(): void {
    // Assuming getRooms is a method that fetches all chatrooms
    this.chatroomService.getRooms().subscribe({
      next: (response) => this.chatrooms = response.data,
      error: (error) => console.error('Error fetching chatrooms:', error)
    });
  }

  selectChatroom(chatroom: Chatroom): void {
    this.selectedChatroom = chatroom;
    // Optionally fetch messages for the selected chatroom here
  }

  fetchCurrentUser(): void {
    this.userService.getProfile().subscribe({
      next: (response) => this.currentUser = response.data,
      error: (error) => console.error('Error fetching user profile:', error)
    });
  }

  // Add properties to the component for binding


// Example sendMessage method


}
