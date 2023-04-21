import {Component} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {SnackBarService} from "../../services/snack-bar.service";
import {Router} from "@angular/router";
import {IChangePassword, ILogin} from "../../models/login";
import {delay} from "rxjs";

@Component({
  selector: 'app-change-password-page',
  templateUrl: './change-password-page.component.html',
  styleUrls: ['./change-password-page.component.scss']
})
export class ChangePasswordPageComponent {
  constructor(private authService: AuthService,
              private formBuilder: FormBuilder,
              private snack: SnackBarService,) {
  }


  public form = this.formBuilder.group({
    currentPassword: ['', [Validators.required, Validators.minLength(6)]],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    repeatPassword: ['', [Validators.required, Validators.minLength(6)]]
  })

  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
      let request: IChangePassword = {
        currentPassword: this.form.controls.currentPassword.value,
        newPassword: this.form.controls.newPassword.value,
        repeatPassword: this.form.controls.repeatPassword.value,
      };
      await this.authService.changePassword(request);
      this.snack.openSnackBar('Parola a fost schimbată cu succes!');

    } catch (e) {
      console.log(e);
      this.snack.showError(e);
    }
  }

}
