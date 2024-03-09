import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

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
      username: new FormControl(null),
      password: new FormControl(null),
    });
  }

  onLogin() {
    console.log(this.loginForm.value);
  }
}
