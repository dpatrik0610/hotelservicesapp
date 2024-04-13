import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WelcomePageComponent } from './welcome-page/welcome-page.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { HeaderComponent } from './header/header.component';
import { RegisterLoginComponent } from './register-login/register-login.component';
import { RoomsComponent } from './rooms/rooms.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: WelcomePageComponent },
  { path: 'about', component: AboutUsComponent },
  { path: 'contact', component: ContactUsComponent },
  { path: 'header', component: HeaderComponent },
  { path: 'auth', component: RegisterLoginComponent },
  { path: 'rooms', component: RoomsComponent },
  { path: 'rooms/:id', component: RoomsComponent },
  { path: '**', redirectTo: '/home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
