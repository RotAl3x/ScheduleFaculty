import {Component, Inject} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ICourseResponse} from "../../../models/course";
import {SnackBarService} from "../../../services/snack-bar.service";
import {CourseService} from "../../../services/course.service";
import {IStudyProgram} from "../../../models/study-program";
import {StudyProgramService} from "../../../services/study-program.service";
import {IUser} from "../../../models/login";
import {AuthService} from "../../../services/auth.service";
import {IHourType} from "../../../models/hour-type";
import {HourTypeService} from "../../../services/hour-type.service";

@Component({
  selector: 'app-course-dialog',
  templateUrl: './course-dialog.component.html',
  styleUrls: ['./course-dialog.component.scss']
})

export class CourseDialogComponent {
  public form = this.formBuilder.group({
    id: ['ddf3c33a-7fa1-442d-9afc-7cac2edb8d3a'],
    studyProgramYearId: ['', [Validators.required]],
    hourTypeIds: [[''], [Validators.required]],
    professorUserId: ['', [Validators.required]],
    name: ['', [Validators.required]],
    abbreviation: ['', [Validators.required]],
    semester: [1, [Validators.required]],
    isOptional: [false],
  })
  public studyPrograms: IStudyProgram[] = []

  public professorUsers: IUser[] = [];

  public hourTypes: IHourType[] = [];

  constructor(
    public dialogRef: MatDialogRef<CourseDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ICourseResponse,
    private formBuilder: FormBuilder,
    private snack: SnackBarService,
    private courseService: CourseService,
    private studyProgramService: StudyProgramService,
    private hourTypeService: HourTypeService,
    private authService: AuthService
  ) {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  async ngOnInit(): Promise<void> {
    this.professorUsers = await this.authService.getUsersByRole('Professor');
    this.studyPrograms = await this.studyProgramService.getAll();
    this.hourTypes = await this.hourTypeService.getAll();
    if (this.data) {
      this.form.patchValue(this.data);
      this.form.controls.studyProgramYearId.setValue(this.data.studyProgram.id);
      this.form.controls.professorUserId.setValue(this.data.professorUser.id);
      let hourTypeIds: string[] = [];
      this.data.hourTypes.forEach(x => {
        if (x.id) hourTypeIds.push(x.id)
      });
      this.form.controls.hourTypeIds.setValue(hourTypeIds);
    }
  }

  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
      if (this.data) {
        await this.courseService.update(this.form.value);
        this.snack.openSnackBar('Clasa a fost actualizată cu succes');
      } else {
        await this.courseService.create(this.form.value);
        this.snack.openSnackBar('Clasa a fost adaugată cu succes');
        this.form.reset();
      }
    } catch (e) {
      this.snack.showError(e);
    }
  }

  public compareFn(optionOne: any, optionTwo: any): boolean {
    return optionOne.id === optionTwo.id;
  }


}
