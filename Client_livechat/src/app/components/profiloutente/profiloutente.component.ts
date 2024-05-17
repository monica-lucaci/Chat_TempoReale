import { Component, OnInit, Input } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ChatroomService } from '../../services/chatroom.service';
import { Chatroom } from '../../models/chatroom';


@Component({
  selector: 'app-profiloutente',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './profiloutente.component.html',
  styleUrl: './profiloutente.component.css'
})
export class ProfiloutenteComponent implements OnInit {
  utente: User | undefined;
  img!: string;
  showOpts: boolean = false;
   currentDate :number = Date.now();
   chatrooms: Chatroom[] = [];

  constructor(
    private userService: UserService,
    private router: Router,
    private authService: AuthService,
    private chatroomService: ChatroomService,
  ) 
  {
  //   if(!localStorage.getItem("token")){
  //     this.router.navigateByUrl("")
  // }
}

  ngOnInit() {
    const username = this.authService.getCurrentUser();
    if (username) {
      this.getProfile(username);
      this.loadChatrooms(username);
    } else {
      console.log('Error: User not found');
    }
  }

  getProfile(username:string){
    this.userService.recuperaProfilo(username).subscribe(res=>{
        this.utente=res.data;
        console.log(this.utente)
        console.log(this.utente?.img)
    })
}

loadChatrooms(username: string): void {
  this.chatroomService.getChatroomsOfUser(username).subscribe(response => {
    this.chatrooms = response.data;
    console.log('Chatrooms:', this.chatrooms); // Debugging log
  });
}


  changeImg() {
    if (this.utente) {
      this.utente.img = this.img;
      this.userService.updateImg(this.utente).subscribe({
        next: (res) => console.log('Image updated successfully'),
        error: (err) => console.error('Failed to update image:', err)
      });
    }
  }

  // logOut() {
  //   console.log('Logging out...');
  //   localStorage.removeItem('token');
  //   this.router.navigateByUrl('');
  // }

  show() {
    this.showOpts = !this.showOpts;
  }
}