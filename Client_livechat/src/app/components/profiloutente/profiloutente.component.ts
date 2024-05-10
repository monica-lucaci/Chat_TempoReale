import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-profiloutente',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profiloutente.component.html',
  styleUrl: './profiloutente.component.css'
})
export class ProfiloutenteComponent implements OnInit {
  user: User | undefined;
  img!: string;
  showOpts: boolean = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    console.log('ngOnInit called');
    this.loadProfile();
  }

  loadProfile() {
    console.log("load profile chiamato")
    // Call the getUserDetail method from the authService to fetch the user profile
    this.authService.getUserDetail().subscribe(
      (response: any) => {
        this.user = response.data; // Assuming the user data is in a property called 'data'
        console.log(this.user);
      },
      (error:any) => {
        console.error('Error fetching user profile:', error);
      }
    );
  }

  changeImg() {
    if (this.user) {
      this.user.img = this.img;
      this.userService.updateImg(this.user).subscribe({
        next: (res) => console.log('Image updated successfully'),
        error: (err) => console.error('Failed to update image:', err)
      });
    }
  }

  logOut() {
    console.log('Logging out...');
    localStorage.removeItem('token');
    this.router.navigateByUrl('');
  }

  show() {
    this.showOpts = !this.showOpts;
  }
}