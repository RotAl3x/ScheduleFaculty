import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import { IAuthSession } from 'src/app/models/login';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent{
  constructor(private authService: AuthService,
  private router:Router) {
  }

  async logout(){
    await this.authService.logout();
  }
}
