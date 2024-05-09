import { Routes } from '@angular/router';
import { LandingpageComponent } from './pages/landingpage/landingpage.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { ForgotpasswordComponent } from './pages/forgotpassword/forgotpassword.component';
import { ProfiloutenteComponent } from './components/profiloutente/profiloutente.component';
import { ChatroomComponent } from './pages/chatroom/chatroom.component';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    {path: 'home', component: LandingpageComponent},
    { path: 'login', component: LoginComponent },
    { path: 'profilo', component: ProfiloutenteComponent },
    { path: 'register', component: RegisterComponent },
    {path: 'chat', component: ChatroomComponent},
    {path: 'resetpassword', component: ForgotpasswordComponent},
    {path: 'userProfile', component: ProfiloutenteComponent}
];
