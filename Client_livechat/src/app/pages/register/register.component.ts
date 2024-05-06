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
  username: string = '';
  password: string = '';
  agree!: boolean;
  constructor(private service: AuthService, private router: Router) {}

  register(): void {
    this.service
      .registra(this.username, this.password)
      .subscribe((result) => {
        this.service.login(this.username, this.password).subscribe(res=>{
          if (res.token) {
            localStorage.setItem('token', res.token);
            this.router.navigateByUrl("/login")
          }
        });
      });
  }
}
