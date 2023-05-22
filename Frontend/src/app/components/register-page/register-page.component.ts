import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {SnackBarService} from "../../services/snack-bar.service";
import {IRole} from "../../models/login";

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent implements OnInit {
  public form = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    role: ['', [Validators.required]]
  })

  roles: string[] = [];

  constructor(private authService: AuthService,
              private formBuilder: FormBuilder,
              private snack: SnackBarService,) {
  }

  async ngOnInit() {
    this.roles = await this.authService.getAllRoles();
  }


  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
      await this.authService.register(this.form.value);
      this.snack.openSnackBar('Cont înregistrat cu succes!');
      this.form.reset();

    } catch (e) {
      this.snack.showError(e);
    }
  }
}
