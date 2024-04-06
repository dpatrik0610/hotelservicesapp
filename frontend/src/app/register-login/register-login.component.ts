import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { passwordMatchValidator } from '../shared/password-match-validator';

@Component({
  selector: 'app-register-login',
  templateUrl: './register-login.component.html',
  styleUrl: './register-login.component.css',
})
export class RegisterLoginComponent {
  passwordValue = '';
  isLoginPage = false;

  loginForm: FormGroup = new FormGroup({});
  signUpForm: FormGroup = new FormGroup({});

  ngOnInit() {
    this.loginForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required),
    });

    this.signUpForm = new FormGroup(
      {
        username: new FormControl(null, Validators.required),
        password: new FormControl(null, Validators.required),
        passwordAgain: new FormControl(null, Validators.required),
      },
      { validators: passwordMatchValidator() }
    );
  }

  onLogin() {
    if (this.isLoginPage) {
      console.log(this.loginForm.value);
      this.loginForm.reset();
    } else {
      this.isLoginPage = !this.isLoginPage;
    }
  }

  onSignup() {
    console.log('this is from onSignup()');
    if (!this.isLoginPage) {
      console.log(this.signUpForm.value);
      this.signUpForm.reset();
    } else {
      this.isLoginPage = !this.isLoginPage;
    }
  }
}
