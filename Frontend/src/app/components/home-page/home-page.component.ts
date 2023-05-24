import {Component, inject, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit{
  authService=inject(AuthService);
  isSecretary:boolean=false;
  isLabAssistant:boolean=false;

  async ngOnInit() {
    this.isSecretary=await this.authService.hasRole('Secretary');
    this.isLabAssistant=await this.authService.hasRole('LabAssistant');
  }

}
