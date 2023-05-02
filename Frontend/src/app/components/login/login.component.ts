import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {FormBuilder, Validators} from "@angular/forms";
import {delay} from "rxjs";
import {SnackBarService} from "../../services/snack-bar.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public form = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  })

  constructor(private authService: AuthService,
              private formBuilder: FormBuilder,
              private snack: SnackBarService,
              private router: Router) {
  }

  async ngOnInit() {
    const session = await this.authService.getSession();
    if (session) {
      await this.router.navigate(['home']);
    }
  }

  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
      await this.authService.login(this.form.value);
      await delay(1000);
      this.snack.openSnackBar('Te-ai logat cu succes');
      await this.router.navigate(['home']);
    } catch (e) {
      this.snack.showError(e);
    }
  }


}
