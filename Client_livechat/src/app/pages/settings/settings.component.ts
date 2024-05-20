import { Component, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ChatroomService } from '../../services/chatroom.service';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PasswordModalComponent } from '../../components/password-modal/password-modal.component';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [FormsModule, CommonModule, PasswordModalComponent],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css',
})
export class SettingsComponent {
  @ViewChild(PasswordModalComponent) passwordModal!: PasswordModalComponent;
  utente: User = {
    user: '',
    email: '',
    pass: '',
    img: '',
    myChatRooms: [],
  };
  newPassword: string = '';
  password: string = ''; // Password for verification
  imgLink: string = '';
  isModalVisible: boolean = false; // Flag to control modal visibility
  modalAction: 'update' | 'delete' = 'update'; // Action type

  constructor(
    private userService: UserService,
    private router: Router,
    private authService: AuthService,
    private chatroomService: ChatroomService,
  ) {}
  ngOnInit() {
    const username = this.authService.getCurrentUser();
    if (username) {
      this.getProfile(username);
    } else {
      console.log('Error: User not found');
    }
  }

  getProfile(username: string) {
    this.userService.recuperaProfilo(username).subscribe((res) => {
      if (res.data) {
        this.utente = res.data;
        if (this.utente.img) this.imgLink = this.utente.img; // Initialize imgLink with the current image if available
      }
    });
  }

  openPasswordModal(action: 'update' | 'delete') {
    this.modalAction = action;
    this.isModalVisible = true;  // Show the modal

  }

  closePasswordModal() {
    this.isModalVisible = false; // Hide the modal
  }

  onPasswordSubmitted(password: string | null) {
    this.closePasswordModal();
    if (password && this.utente) {
      if (this.modalAction === 'update') {
        const updatedUser = { ...this.utente, pass: password };
        this.userService.updateImg(updatedUser, this.imgLink).subscribe({
          next: res => {
            console.log('Image updated successfully');
            alert('Image updated successfully'); // Show success message
            if(this.utente.user)
              this.getProfile(this.utente.user);
          },
          error: err => {
            console.error('Failed to update image:', err);
            alert('Failed to update image. Please try again.'); // Show error message
          }
        });
      } else if (this.modalAction === 'delete') {
        const userWithPassword = { ...this.utente, pass: password };
        this.userService.deleteImage(userWithPassword).subscribe({
          next: res => {
            console.log('Image deleted successfully');
            alert('Image deleted successfully'); // Show success message
            if(this.utente.user)
              this.getProfile(this.utente.user);
          },
          error: err => {
            console.error('Failed to delete image:', err);
            alert('Failed to delete image. Please try again.'); // Show error message
          }
        });
      }
    } else if (password === '') {
      console.log('Password submission was cancelled.');
    } else {
      console.error('Password is required to update or delete the image.');
    }
  }

  deleteImg() {
    this.openPasswordModal('delete');
  }

  updateImg() {
    this.openPasswordModal('update');
  }


  // changeImg() {
  //   if (this.utente && this.utente.user) {
  //     const updatedUser = { ...this.utente, pass: this.password };
  //     if(this.imgLink)
  //       this.userService.updateImg(updatedUser, this.imgLink).subscribe({
  //         next: res => console.log('Image updated successfully'),
  //         error: err => console.error('Failed to update image:', err)
  //       });
  //   } else {
  //     console.error('User information is not available');
  //   }
  // }

  // deleteImg() {
  //   if (this.utente && this.utente.user) {
  //     this.userService.deleteImage(this.utente.user).subscribe({
  //       next: (res) => {
  //         console.log('Image deleted successfully');
  //         this.utente.img = '../../../assets/img-chat/defaultUser.jpg';
  //       },
  //       error: (err) => console.error('Failed to delete image:', err),
  //     });
  //   } else {
  //     console.error('User information is not available');
  //   }
  // }

  resetPassword() {
    if (this.utente && this.utente.user && this.newPassword) {
      this.userService
        .resetPassword(this.utente.user, this.newPassword)
        .subscribe({
          next: (res) => console.log('Password reset successfully'),
          error: (err) => console.error('Failed to reset password:', err),
        });
    } else {
      console.error('User information or new password is not available');
    }
  }

  deleteAccount() {
    if (this.utente && this.utente.user) {
      this.userService.deleteUser(this.utente.user).subscribe({
        next: (res) => {
          console.log('Account deleted successfully');
          this.authService.logout();
          this.router.navigate(['/']);
        },
        error: (err) => console.error('Failed to delete account:', err),
      });
    } else {
      console.error('User information is not available');
    }
  }

  save() {}
}
