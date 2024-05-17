import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-logout-modal',
  standalone: true,
  imports: [ CommonModule, FormsModule],
  templateUrl: './logout-modal.component.html',
  styleUrl: './logout-modal.component.css'
})
export class LogoutModalComponent {
  @Input('showLogout')
  showLogout=true;

  @Output('close')
  onClose = new EventEmitter()

  constructor(private authService : AuthService,    private router: Router,){

  }

  onLogout():void{
    this.authService.logout();
    this.router.navigateByUrl('/home');
  }
}
