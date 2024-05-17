import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ChatroomService } from '../../services/chatroom.service';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent {
  utente: User | undefined;
  img!: string;
  constructor(
    private userService: UserService,
    private router: Router,
    private authService: AuthService,
    private chatroomService: ChatroomService,
  ) 
  {}
  ngOnInit() {
    const username = this.authService.getCurrentUser();
    if (username) {
      this.getProfile(username);
  
    } else {
      console.log('Error: User not found');
    }
  }

  getProfile(username:string){
    this.userService.recuperaProfilo(username).subscribe(res=>{
        this.utente=res.data;
        //console.log(this.utente)
        //console.log(this.utente?.img)
    })
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


}
