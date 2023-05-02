import {inject, Injectable} from '@angular/core';
import {AuthService} from "./auth.service";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class NavigateService {

  private authService = inject(AuthService);
  private router = inject(Router)

  constructor() {

  }

  async toHome() {
    const session = await this.authService.getSession();
    if (session) {
      await this.router.navigate(['home']);
    }
  }
}
