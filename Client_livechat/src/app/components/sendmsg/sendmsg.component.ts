import { Component } from '@angular/core';
import { User } from '../../models/user';
import { SendmsgService } from '../../services/sendmsg.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-sendmsg',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './sendmsg.component.html',
  styleUrl: './sendmsg.component.css'
})
export class SendmsgComponent {
  text: string = '';
  id!: string;
  currentUser!:User;


  constructor(
    private messageService: SendmsgService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private userSvc : UserService
  ) {}

  ngOnInit(): void {
    this.activeRoute.params.subscribe((p) => {
      this.id = p['cd'];
    });
    // this.userSvc.getProfile().subscribe(res=>{
    //   this.currentUser = res.data;
    // })
  }

  sendMessage() {

    this.messageService
      .sendMessage(this.id, this.text, this.currentUser.img!)
      .subscribe(
        (response) => {
          console.log('Messaggio inviato con successo:', response);
        },
        (error) => {
          console.error("Errore durante l'invio del messaggio:", error);
        }
      );
    this.text = '';
  }

}
