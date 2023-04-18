import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  constructor(private authService:AuthService) {
  }

  async submit(){
    try{
      let test={
        email: 'string',
        password: 'string',
      }
      await this.authService.login(test);
    }
    catch (e){
      console.log(e);
    }
  }

}
