import {Component} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {SnackBarService} from "../../services/snack-bar.service";

@Component({
  selector: 'app-change-password-page',
  templateUrl: './change-password-page.component.html',
  styleUrls: ['./change-password-page.component.scss']
})
export class ChangePasswordPageComponent {
  public form = this.formBuilder.group({
    currentPassword: ['', [Validators.required, Validators.minLength(6)]],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    repeatPassword: ['', [Validators.required, Validators.minLength(6)]]
  })

  constructor(private authService: AuthService,
              private formBuilder: FormBuilder,
              private snack: SnackBarService,) {
  }

  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
      await this.authService.changePassword(this.form.value);
      this.snack.openSnackBar('Parola a fost schimbată cu succes!');

    } catch (e) {
      this.snack.showError(e);
    }
  }

}
