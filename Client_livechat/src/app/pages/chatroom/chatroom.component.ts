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
import { MatSnackBar } from '@angular/material/snack-bar';
import { LogoutModalComponent } from '../../components/logout-modal/logout-modal.component'
import { EditMessageModalComponent } from '../../components/edit-message-modal/edit-message-modal.component';
import { ProfiloutenteComponent } from '../../components/profiloutente/profiloutente.component';

@Component({
  selector: 'app-chatroom',
  standalone: true,
  imports: [ProfiloutenteComponent,SendmsgComponent, CommonModule, FormsModule,CreatechatComponent,AddusertochatComponent,EditMessageModalComponent,LogoutModalComponent],
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
  pollingInterval = 20000; // Added polling interval property
  private pollingSubscription: Subscription | undefined; // Added subscription for polling
  searchQuery: string = '';
  searchQuery2: string= '';
  isSearchInputActive: boolean = false;
  isDropdownVisible: boolean = false;
  isDropdown2Visible: boolean = false;
  canShowModal = false;
  canShowModal2= false;
  canShowModal3= false;

  // canShowPopup = false;
  // @Output() showPopup: EventEmitter<boolean> = new EventEmitter<boolean>();
  

  messagePopups: boolean[] = [];


  showEditModal: boolean = false;
  selectedMessage: any = null;

  constructor(
    private chatroomService: ChatroomService,
    private authService: AuthService,
    private userService: UserService,
    private msgService: MessageService,
    private snackBar: MatSnackBar 
  ) {}

  ngOnInit(): void {
 
    console.log('Chatrooms:', this.chatrooms)
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
          console.log(this.chatrooms);
          const lastChatroomId = localStorage.getItem('lastChatroomId');
          if (lastChatroomId) {
            const lastChatroom = this.chatrooms.find(
              (chatroom) => chatroom.crCd === lastChatroomId
            );
            if (lastChatroom) {
              this.showChatMessages(lastChatroom);
            } else if (this.chatrooms.length > 0) {
              this.showChatMessages(this.chatrooms[0]);
            } else {
              this.selectedChatroom = null;
              this.messages = [];
            }
          } else if (this.chatrooms.length > 0) {
            this.showChatMessages(this.chatrooms[0]);
          } else {
            this.selectedChatroom = null;
            this.messages = [];
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
          console.log("the code is "+ code)
          const images = this.selectedChatroom?.imgU;
          console.log("the img is "+ images)
         // console.log('lets check the arrays '+this.selectedChatroom + ' another one  '+ this.selectedChatroom?.imgU)
        //  console.log(this.selectedChatroom);
          if (code) {
            // Ensure that code is defined before passing it
            this.getMessagesForRoom(code);
            this.getChatRoomImgs(code)
      
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

  toggleDropdown() {
    this.isDropdownVisible = !this.isDropdownVisible;
    //toggle for the 3 dott svg in the left-settings and logout
  }
  toggleDropdown2() {
    this.isDropdown2Visible = !this.isDropdown2Visible;
    //toggle for the delete chatroom svg
  }
  
  togglePopup(index: number) {
    // Toggle the popup state for the clicked message
    this.messagePopups[index] = !this.messagePopups[index];
  }


  copyMessage(message: string | undefined, event: MouseEvent, index: number) {
    event.preventDefault();
    if (!message) {
      console.error('Message is undefined or empty');
      return;
    }
  
    // Check if the browser supports the Clipboard API
    if (navigator.clipboard) {
      // Write the message to the clipboard
      navigator.clipboard.writeText(message)
        .then(() => {
          console.log('Message copied to clipboard:', message);
          // Close the popup for the clicked message
          this.messagePopups[index] = false;
          // Optionally, provide feedback to the user
          // e.g., show a toast message
        })
        .catch((error) => {
          console.error('Failed to copy message:', error);
          // Handle errors, e.g., show an error message to the user
        });
    } else {
      // Fallback for browsers that don't support the Clipboard API
      console.error('Clipboard API not supported');
      // You can provide a fallback mechanism here, e.g., prompt the user to copy manually
    }
  }
  

  deleteMessage(messageCode: string | undefined , username: string |undefined , event: MouseEvent,index: number): void {
    event.preventDefault();
    if (messageCode && username) {
      this.msgService.deleteMessage(messageCode, username).subscribe(
        (response) => {
          // Handle successful deletion
          this.snackBar.open('Message deleted successfully', 'Close', {
            duration: 3000, // Duration in milliseconds
          });

          if (this.selectedChatroom?.crCd) {
            this.getMessagesForRoom(this.selectedChatroom.crCd);
          }
          this.messagePopups[index] = false;
          // Refresh messages or perform any other necessary actions
        },
        (error) => {
          console.error('Error deleting message:', error);
          // Handle error if deletion fails
        }
      );
    }


  
  }


  editMessage(message: any,event: MouseEvent, index:number): void {
    event.preventDefault();
    this.selectedMessage = message;
    this.showEditModal = true;
    this.messagePopups[index] = false;
  }

  closeEditModal(): void {
    this.showEditModal = false;
    this.selectedMessage = null;
  }

  saveEditedMessage(newText: string): void {
   
    if (this.selectedMessage) {
      this.msgService.updateMessage(this.selectedMessage.messageCode, this.selectedMessage.sender, newText)
        .subscribe(response => {
          this.snackBar.open('Message updated successfully', 'Close', {
            duration: 3000, // Duration in milliseconds
          });
          if (this.selectedChatroom?.crCd) {
            this.getMessagesForRoom(this.selectedChatroom.crCd);
          }
          //  this.messagePopups[index] = false;
        },
        (error) => {
          console.error('Error deleting message:', error);
        }
      );
    }
  }

  clearRoom(cr_code:string | undefined): void {
    if(cr_code)
      this.chatroomService.clearRoom(cr_code).subscribe(
        response => {
          this.snackBar.open('Messages canceled', 'Close', {
            duration: 3000,
          });
          if (this.selectedChatroom?.crCd) {
            this.getMessagesForRoom(this.selectedChatroom.crCd);
          }
       

      },
      (error) => {
        console.error('Error deleting messages:', error);
      }
    )
  }

  deleteRoom(cr_code: string | undefined, username: string | undefined): void {
    if (cr_code && username) {
      this.chatroomService.deleteChatRoom(cr_code, username).subscribe(
        response => {
          this.snackBar.open('Chatroom deleted successfully', 'Close', {
            duration: 3000,
          });
          this.loadChatrooms();
        },
        error => {
          console.error('Error deleting room:', error);
          // Display an error message to the user
          this.snackBar.open('Error deleting chatroom', 'Close', {
            duration: 3000,
          });
        }
      );
    } else {
      console.error('Chatroom code or username is undefined');
    }
  }
  getChatRoomImgs(cr_code: string | undefined): void {
    if (!cr_code) {
      console.error('Chatroom code is undefined');
      return;
    }
  
    this.chatroomService.getChatroomAndMessages(cr_code).subscribe(
      response => {
        const chatroomData = response.data;
        console.log(chatroomData.imgU[1] + " chatroom data is")
        if (this.selectedChatroom) {
          this.selectedChatroom.usrs = chatroomData.usrs;
          this.selectedChatroom.imgU = chatroomData.imgU;
          if(this.selectedChatroom?.imgU)
            console.log(this.selectedChatroom?.imgU[1] + " chatroom data is")
        }
  
     
     
      },
      error => {
        console.error('Error getting rooms images:', error);
       
      }
    );
  }
  
  

  onChatroomCreated(newChatroom: Chatroom) {
    this.chatrooms.push(newChatroom);
    this.filteredChatrooms.push(newChatroom);
    this.loadChatrooms();
    this.showChatMessages(newChatroom);
  }

  getUserImage(username: string | undefined): string | undefined {
    if (!this.selectedChatroom || !username) return undefined;
  
    const userIndex = this.selectedChatroom.usrs?.indexOf(username);
    if (userIndex !== undefined && userIndex !== -1) {
      return this.selectedChatroom.imgU?.[userIndex];
    }
    return undefined;
  }
  



}  
