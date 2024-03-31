import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register-login',
  templateUrl: './register-login.component.html',
  styleUrl: './register-login.component.css',
})
export class RegisterLoginComponent {
  passwordValue = '';
  isLoginPage = true;

  loginForm: FormGroup = new FormGroup({});

  ngOnInit() {
    this.loginForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required),
      passwordAgain: new FormControl(null),
    });
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
    this.isLoginPage = !this.isLoginPage;
    console.log(this.isLoginPage);
    // console.log('isLoginPage value before making it false: ', this.isLoginPage);
    // if (this.isLoginPage) this.isLoginPage = false;
    // console.log('isLoginPage value after making it false: ', this.isLoginPage);
    // if (!this.isLoginPage) {
    //   if (this.loginForm.get('passwordAgain')) {
    //     this.loginForm?.removeControl('passwordAgain');
    //   } else {
    //     this.loginForm.addControl(
    //       'passwordAgain',
    //       new FormControl(null, Validators.required)
    //     );
    //   }
    // }
  }
}
