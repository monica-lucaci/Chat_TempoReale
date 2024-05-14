import { Component, OnInit } from '@angular/core';
import { Chatroom } from '../../models/chatroom';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { ChatroomService } from '../../services/chatroom.service';
import { interval, Subscription } from 'rxjs';
import { SendmsgComponent } from '../../components/sendmsg/sendmsg.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Message } from '../../models/message';
import { AuthService } from '../../services/auth.service';
import { MessageService } from '../../services/message.service';
import { CreatechatComponent } from '../../components/createchat/createchat.component';
import { EventEmitter, Output } from '@angular/core';
import { AddusertochatComponent } from '../../components/addusertochat/addusertochat.component';

@Component({
  selector: 'app-chatroom',
  standalone: true,
  imports: [SendmsgComponent, CommonModule, FormsModule,CreatechatComponent,AddusertochatComponent],
  templateUrl: './chatroom.component.html',
  styleUrl: './chatroom.component.css',
})
export class ChatroomComponent implements OnInit {
  chatrooms: Chatroom[] = [];
  selectedChatroom: Chatroom | null = null;
  filteredChatrooms: Chatroom[] = [];
  messages: Message[] = [];
  currentUser: string | null = null; // Initialize here
  utente: User | undefined;
  //isSelected: boolean = false; // Initially, no item is selected
  pollingInterval = 90000; // Added polling interval property
  private pollingSubscription: Subscription | undefined; // Added subscription for polling
  searchQuery: string = '';
  searchQuery2: string= '';
  isSearchInputActive: boolean = false;

  canShowModal = false;
  canShowModal2= false;
  // canShowPopup = false;
  // @Output() showPopup: EventEmitter<boolean> = new EventEmitter<boolean>();

  messagePopups: boolean[] = [];

  constructor(
    private chatroomService: ChatroomService,
    private authService: AuthService,
    private userService: UserService,
    private msgService: MessageService,
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    console.log(this.currentUser);

    const username = this.authService.getCurrentUser();
    if (username) {
      this.getProfile(username);
    } else console.log('error');
    this.startPolling();
  }


  ngOnDestroy(): void {
    // Stop polling when the component is destroyed
    this.stopPolling();
  }

  startPolling(): void {
    this.pollingSubscription = interval(this.pollingInterval).subscribe(() => {
      if (this.selectedChatroom?.crCd) {
        this.getMessagesForRoom(this.selectedChatroom.crCd);
      }
    });
  }

  stopPolling(): void {
    if (this.pollingSubscription) {
      this.pollingSubscription.unsubscribe();
    }
  }

  getProfile(username: string) {
    this.userService.recuperaProfilo(username).subscribe((res) => {
      this.utente = res.data;
     // console.log(this.utente);
     // console.log(this.utente?.img);
    });
    this.loadChatrooms();
  }

  loadChatrooms() {
    if (this.currentUser) {
      this.chatroomService
        .getChatroomsOfUser(this.currentUser)
        .subscribe((response) => {
          this.chatrooms = response.data;
          const lastChatroomId = localStorage.getItem('lastChatroomId');
          if (lastChatroomId) {
            const lastChatroom = this.chatrooms.find(
              (chatroom) => chatroom.crCd === lastChatroomId,
            );
            if (lastChatroom) {
              this.showChatMessages(lastChatroom);
            }
          }
          
          // Apply filtering logic here
          this.applyFilter();

        });
    }
  }

  showChatMessages(chatroom: Chatroom | null) {
    if (chatroom && chatroom.crCd) {
      this.chatroomService
        .getChatroomAndMessages(chatroom.crCd)
        .subscribe((response) => {
          this.selectedChatroom = chatroom;
          if (chatroom.crCd)
            localStorage.setItem('lastChatroomId', chatroom.crCd);
          const code = this.selectedChatroom?.crCd;
        //  console.log(this.selectedChatroom);
          if (code) {
            // Ensure that code is defined before passing it
            this.getMessagesForRoom(code);
          }
        });
    }
  }

  getMessagesForRoom(crCode: string): void {
    this.msgService.getMessagesOfRoom(crCode).subscribe(
      (response) => {
        this.messages = response.data;
       // console.log(this.messages[0]);
        // Assuming the response directly contains messages
      },
      (error) => {
        console.error('Error loading messages for room:', error);
      },
    );
  }

  applyFilter() {
    // Apply filtering logic here whenever searchQuery changes
    this.filteredChatrooms = this.chatrooms.filter(
      (chatroom) =>
        chatroom && chatroom.titl && chatroom.titl.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
   // console.log('Search Query:', this.searchQuery);
   // console.log('Chatrooms:', this.chatrooms);
  }

  highlightKeyword(message: string, keyword: string): string {
    if (!keyword) return message;
    const regex = new RegExp(keyword, 'gi');
    return message.replace(regex, (match) => `<span class="text-yellow-500">${match}</span>`);
  }

  getMessageStyle(message: Message): any {
    if (!this.searchQuery2 || !message.data) return {};
    const regex = new RegExp(this.searchQuery2, 'gi');
    return regex.test(message.data) ? { backgroundColor: 'blue' } : {};
  }

  toggleSearchInput() {
    this.isSearchInputActive = !this.isSearchInputActive;
    // Reset the search query when toggling
    if (!this.isSearchInputActive) {
      this.searchQuery2 = ''; // Reset the search query
      this.applyFilter(); // Reapply any existing filter
    }
  }

  // togglePopup() {
  //   this.canShowPopup = !this.canShowPopup;
  //   // Emitting the boolean value to parent component
  //   this.showPopup.emit(this.canShowPopup);
  // }
  togglePopup(index: number) {
    // Toggle the popup state for the clicked message
    this.messagePopups[index] = !this.messagePopups[index];
  }
}
