import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register-login',
  templateUrl: './register-login.component.html',
  styleUrl: './register-login.component.css',
})
export class RegisterLoginComponent {
  passwordValue = '';

  loginForm: FormGroup = new FormGroup({});

  ngOnInit() {
    this.loginForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required),
    });
  }

  onLogin() {
    console.log(this.loginForm.value);
    this.loginForm.reset();
  }
}
