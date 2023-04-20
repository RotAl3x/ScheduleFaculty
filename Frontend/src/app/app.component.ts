import {Component, OnInit} from '@angular/core';
import {AuthService} from "./services/auth.service";
import {NavigationEnd, Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Frontend';

  constructor(private router:Router) {
  }

  isOnLogin(){
    return this.router.url === '/login'
  }
}
