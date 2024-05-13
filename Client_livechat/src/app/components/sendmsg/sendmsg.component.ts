import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from '../../models/user';
import { MessageService } from '../../services/message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatroomComponent } from '../../pages/chatroom/chatroom.component';
import { Chatroom } from '../../models/chatroom';

@Component({
  selector: 'app-sendmsg',
  standalone: true,
  imports: [CommonModule,FormsModule, ChatroomComponent],
  templateUrl: './sendmsg.component.html',
  styleUrl: './sendmsg.component.css'
})
export class SendmsgComponent {
  @Input() currentUser: string | null = null;; // Define currentUser as an input property
  @Input() selectedChatroom:  Chatroom | null = null;
  text: string = '';
  @Output() messageSent = new EventEmitter<void>();

  constructor(private messageService: MessageService) {}


  sendMessage() {
   
    if (this.text.trim() !== '') {
      const crCode = this.selectedChatroom?.crCd
      console.log(this.selectedChatroom?.crCd)
      const message = {
        data: this.text.trim()
      };
      if (this.currentUser && crCode) { // Check if currentUser is not null

        console.log("this is the curent user in message "+ this.currentUser)
        this.messageService.sendMessage(message, crCode, this.currentUser).subscribe(response => {
          console.log('Message sent:', response);
          this.text = ''; // Clear the input field after sending

          this.messageSent.emit();
        }, error => {
          console.error('Error sending message:', error);
        });
      } else {
        console.error('Current user is null');
      }


      
    }

  }
  
}