
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OnInit, Inject, PLATFORM_ID, inject, Component } from '@angular/core';

import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  email: string = '';
  user: string = '';
  pass: string = '';
  matSnackBar = inject(MatSnackBar);
  hide = true;
  form!: FormGroup;
  fb = inject(FormBuilder);

  constructor(
    private authService: AuthService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {

    if (localStorage.getItem('token')) router.navigateByUrl('/userProfile');
  }

  onLogin() {
    console.log('Login function triggered');
    console.log(
      'Email:',
      this.email,
      'User:',
      this.user,
      'Password:',
      this.pass,
    );
    this.authService.login(this.email, this.user, this.pass).subscribe({
      next: (token) => {
        localStorage.setItem('token', token);
        console.log('Received token:', token);
        this.router.navigateByUrl("/userProfile");
      },
      error: (error) => {
        console.error('Error during login:', error);
      }
    });
  }


  ngOnInit(): void {
   
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      user: ['', [Validators.required]],
      pass: ['', [Validators.required]]
    });
  
  }
}
