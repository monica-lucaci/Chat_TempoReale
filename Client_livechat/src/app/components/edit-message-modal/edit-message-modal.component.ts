import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-message-modal',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './edit-message-modal.component.html',
  styleUrl: './edit-message-modal.component.css'
})
export class EditMessageModalComponent {
  @Input() show: boolean = false;
  @Input() message: string = '';
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<string>();

  editedMessage: string = '';

  ngOnChanges(): void {
    this.editedMessage = this.message;
  }

  close(): void {
    this.onClose.emit();
  }

  save(): void {
    this.onSave.emit(this.editedMessage);
    this.onClose.emit();
  }
}
