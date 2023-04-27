import {RouterModule, Routes} from "@angular/router";
import {inject, NgModule} from "@angular/core";
import {LoginComponent} from "./components/login/login.component";
import {HomePageComponent} from "./components/home-page/home-page.component";
import {ChangePasswordPageComponent} from "./components/change-password-page/change-password-page.component";
import {ClassroomPageComponent} from "./components/classroom-page/classroom-page.component";
import {CoursePageComponent} from "./components/course-page/course-page.component";

const routes: Routes = [
  {path:'login',component:LoginComponent},
  {path:'home',component:HomePageComponent},
  {path:'change-password', component:ChangePasswordPageComponent},
  {path:'classroom', component:ClassroomPageComponent},
  {path:'course', component:CoursePageComponent},
  {path:'',redirectTo:'/login',pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
