import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-password-modal',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './password-modal.component.html',
  styleUrl: './password-modal.component.css'
})
export class PasswordModalComponent {
  password: string = '';

  @Output() passwordSubmitted: EventEmitter<string | null> = new EventEmitter<string | null>();

  submitPassword() {
    this.passwordSubmitted.emit(this.password);
    console.log('password ok')

  }

  closeModal() {
    this.passwordSubmitted.emit('');  // Emit empty string when modal is closed without submitting
  }
}