import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  authService = inject(AuthService);
  matSnackBar = inject(MatSnackBar)
  hide = true;
  form!: FormGroup;
  fb = inject(FormBuilder);
  router = inject(Router)

  login() {
    this.authService.login(this.form.value).subscribe({
      next: (response) => {
        this.matSnackBar.open(response.message, 'Close', {
          duration: 5000,
          horizontalPosition: 'center'
        })
        this.router.navigate(['/'])
      },
      error: (er) => {
        this.matSnackBar.open(er.error.message, 'Close', {
          duration: 5000,
          horizontalPosition: 'center'
        });
      },
    });
  }
  onSignIn(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/chat']); // Navigate when logged in
    } else {
      console.error('User is not logged in');
      // Optionally perform the login here or show an error
    }
  }   

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }
}
