import { NgModule } from '@angular/core';
import {
  BrowserModule,
  provideClientHydration,
} from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WelcomePageComponent } from './welcome-page/welcome-page.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { HeaderComponent } from './header/header.component';
import { RegisterLoginComponent } from './register-login/register-login.component';
import {
  HttpClientModule,
  provideHttpClient,
  withFetch,
} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

//PrimeNG modules
import { ButtonModule } from 'primeng/button';
import { AutoFocusModule } from 'primeng/autofocus';
import { InputTextModule } from 'primeng/inputtext';
import { ScrollTopModule } from 'primeng/scrolltop';
import { PasswordModule } from 'primeng/password';
import { ImageSliderComponent } from './welcome-page/image-slider/image-slider.component';

@NgModule({
  declarations: [
    AppComponent,
    WelcomePageComponent,
    AboutUsComponent,
    ContactUsComponent,
    HeaderComponent,
    RegisterLoginComponent,
    ImageSliderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ButtonModule,
    AutoFocusModule,
    InputTextModule,
    ScrollTopModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    PasswordModule,
    ReactiveFormsModule,
  ],
  providers: [provideClientHydration(), provideHttpClient(withFetch())],
  bootstrap: [AppComponent],
})
export class AppModule {}
