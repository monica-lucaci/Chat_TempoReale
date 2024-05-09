import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  protected readonly localStorage = localStorage;

  constructor(private userService: UserService,private router:Router) {}

  ngOnInit() {
    this.getUserProfile();
  }

  getUserProfile() {
    this.userService.getProfile().subscribe({
      next: (response) => this.userService = response.data,
      error: (err) => console.error('Error fetching user profile:', err)
    });
  }

  changeImg() {
    this.user!.img = this.img;
    this.userService.updateImg(this.user!).subscribe({
      next: (res) => console.log('Image updated successfully'),
      error: (err) => console.error('Failed to update image:', err)
    });
  }

  logOut() {
    localStorage.removeItem("token")
    this.router.navigateByUrl("")
  }

  show() {
    this.showOpts = !this.showOpts;
  }
}