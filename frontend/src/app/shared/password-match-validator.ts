import { AbstractControl, ValidatorFn } from '@angular/forms';

export function passwordMatchValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const password = control.get('password');
    const passwordAgain = control.get('passwordAgain');

    if (password && passwordAgain && password.value !== passwordAgain.value) {
      return { passwordMismatch: true };
    }
    return null;
  };
}
