import {RouterModule, Routes} from "@angular/router";
import {inject, NgModule} from "@angular/core";
import {LoginComponent} from "./components/login/login.component";
import {HomePageComponent} from "./components/home-page/home-page.component";
import {NavigateService} from "./services/navigate.service";
import {MenuComponent} from "./components/menu/menu.component";

const routes: Routes = [
  {path:'login',component:LoginComponent},
  {path:'home',component:HomePageComponent},
  {path:'menu',component:MenuComponent},
  {path:'',redirectTo:'/login',pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
