import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {LoginComponent} from "./components/login/login.component";
import {HomePageComponent} from "./components/home-page/home-page.component";
import {ChangePasswordPageComponent} from "./components/change-password-page/change-password-page.component";
import {ClassroomPageComponent} from "./components/classroom-page/classroom-page.component";
import {CoursePageComponent} from "./components/course-page/course-page.component";
import {StudyProgramPageComponent} from "./components/study-program-page/study-program-page.component";
import {StatusPageComponent} from "./components/status-page/status-page.component";
import {HourTypePageComponent} from "./components/hour-type-page/hour-type-page.component";
import {SchedulePageComponent} from "./components/schedule-page/schedule-page.component";
import {RegisterPageComponent} from "./components/register-page/register-page.component";
import {AddUserToACourseComponent} from "./components/add-user-to-a-course/add-user-to-a-course.component";

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'home', component: HomePageComponent},
  {path: 'change-password', component: ChangePasswordPageComponent},
  {path: 'classroom', component: ClassroomPageComponent},
  {path: 'course', component: CoursePageComponent},
  {path: "study", component: StudyProgramPageComponent},
  {path: "status", component: StatusPageComponent},
  {path:"hour-type", component:HourTypePageComponent},
  {path:"schedule",component:SchedulePageComponent},
  {path:"register",component:RegisterPageComponent},
  {path:"add-course",component:AddUserToACourseComponent},
  {path: '', redirectTo: '/login', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
