import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthguardService implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  canActivate(): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    } 
    if (this.authService.isTokenExpired()) {
        // Token is expired, redirect to login page
       // this.router.navigateByUrl('/login');
      }
    return false;
    
  }
}
