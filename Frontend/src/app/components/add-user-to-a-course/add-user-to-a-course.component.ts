import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {IClassroom} from "../../models/classroom";
import {SnackBarService} from "../../services/snack-bar.service";
import {ClassroomService} from "../../services/classroom.service";
import {CourseService} from "../../services/course.service";
import {AuthService} from "../../services/auth.service";
import {IUser} from "../../models/login";
import {ICourse, ICourseResponse} from "../../models/course";
import {AssignedCourseUserService} from "../../services/assigned-course-user.service";

@Component({
  selector: 'app-add-user-to-a-course',
  templateUrl: './add-user-to-a-course.component.html',
  styleUrls: ['./add-user-to-a-course.component.scss']
})
export class AddUserToACourseComponent implements OnInit{
  public form = this.formBuilder.group({
    professorUserId: ['',[Validators.required]],
    courseId: ['',[Validators.required]],
  })

  public users:IUser[]=[];
  public courses:ICourseResponse[]=[];
  constructor(
    private formBuilder: FormBuilder,
    private snack: SnackBarService,
    private courseService:CourseService,
    private assignedCourseUserService:AssignedCourseUserService,
    private authService:AuthService,
  ) {
  }

  async ngOnInit() {
    this.users = await this.authService.getUsersByRole('LabAssistant');
    this.courses = await this.courseService.getAll();
  }

  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
        await this.assignedCourseUserService.create(this.form.value);
        this.snack.openSnackBar('A fost adaugată cu succes');
        this.form.reset();

    } catch (e) {
      this.snack.showError(e);
    }
  }

}
