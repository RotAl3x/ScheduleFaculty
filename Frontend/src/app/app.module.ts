import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {RouterOutlet} from "@angular/router";
import { LoginComponent } from './components/login/login.component';
import {HttpClientModule} from "@angular/common/http";
import {MatFormFieldModule} from "@angular/material/form-field";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatInputModule} from "@angular/material/input";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import { HomePageComponent } from './components/home-page/home-page.component';
import { MenuComponent } from './components/menu/menu.component';
import { ChangePasswordPageComponent } from './components/change-password-page/change-password-page.component';
import { ClassroomPageComponent } from './components/classroom-page/classroom-page.component';
import {MatCardModule} from "@angular/material/card";
import { ClassroomDialogComponent } from './components/classroom-page/classroom-dialog/classroom-dialog.component';
import {MatDialogModule} from "@angular/material/dialog";
import {MatSelectModule} from "@angular/material/select";
import {CoursePageComponent} from "./components/course-page/course-page.component";
import {CourseDialogComponent} from "./components/course-page/course-dialog/course-dialog.component";
import {MatCheckboxModule} from "@angular/material/checkbox";
import { StudyProgramPageComponent } from './components/study-program-page/study-program-page.component';
import { StudyProgramDialogComponent } from './components/study-program-page/study-program-dialog/study-program-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomePageComponent,
    MenuComponent,
    ChangePasswordPageComponent,
    ClassroomPageComponent,
    ClassroomDialogComponent,
    CoursePageComponent,
    CourseDialogComponent,
    StudyProgramPageComponent,
    StudyProgramDialogComponent
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        RouterOutlet,
        MatFormFieldModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatInputModule,
        MatSnackBarModule,
        BrowserAnimationsModule,
        FormsModule,
        MatCardModule,
        MatDialogModule,
        MatSelectModule,
        MatCheckboxModule,
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
