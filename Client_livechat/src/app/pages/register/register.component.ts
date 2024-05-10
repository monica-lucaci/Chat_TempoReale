import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  img: string='';
  email: string= '';
  user: string = '';
  pass: string = '';
  agree!: boolean;
  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    console.log("Agree status:", this.agree);
    if (!this.agree) {
      alert('Please agree to the terms.');
      return;
    }
    this.authService.registra(this.email, this.user, this.pass, this.img).subscribe({
      next: (response: any) => {
        if (response.status === "SUCCESS") {
          // Registration successful, handle further logic if needed
          console.log("Registration successful");
          this.authService.login(this.email, this.user, this.pass).subscribe({
            next: (token: string) => { // Directly handle token as a string
              localStorage.setItem('token', token);
              this.router.navigateByUrl("/login"); // Navigate to dashboard
            },
            error: error => console.error('Login after registration failed:', error)
          });
        } else {
          // Registration failed
          console.error('Registration failed:', response.data);
        }
      },
      error: error => console.error('Registration failed:', error)
    });
  }
  
}
