import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';



import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  matSnackBar = inject(MatSnackBar)
  hide = true;
  form!: FormGroup;
  fb = inject(FormBuilder);


  constructor(private authService: AuthService, private router: Router) {
    if(localStorage.getItem("ilToken"))
      router.navigateByUrl("/profilo")
  }


  login() {
    this.authService.login(this.password,this.username).subscribe(
    (result) => {
      if(result.token){
        localStorage.setItem('token', result.token);
        this.router.navigateByUrl("/chat")
      }
    });
  }
  // onSignIn(): void {
  //   if (this.authService.isLoggedIn()) {
  //     this.router.navigate(['/chat']); // Navigate when logged in
  //   } else {
  //     console.error('User is not logged in');
  //     // Optionally perform the login here or show an error
  //   }
  // }   

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }
}
