import {Component} from '@angular/core';
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {MatDialogRef} from "@angular/material/dialog";
import {SnackBarService} from "../../../services/snack-bar.service";
import {ScheduleService} from "../../../services/schedule.service";
import {ICourseHourType} from "../../../models/course-hour-type";
import {AuthService} from "../../../services/auth.service";
import {IUser} from "../../../models/login";
import {combineLatest, filter} from "rxjs";
import {StudyProgramService} from "../../../services/study-program.service";
import {CourseHourTypeService} from "../../../services/course-hour-type.service";
import {CourseService} from "../../../services/course.service";
import {IStudyProgram} from "../../../models/study-program";
import {ICourseResponse} from "../../../models/course";
import {IClassroom} from "../../../models/classroom";
import {ClassroomService} from "../../../services/classroom.service";

@Component({
  selector: 'app-schedule-dialog',
  templateUrl: './schedule-dialog.component.html',
  styleUrls: ['./schedule-dialog.component.scss']
})
export class ScheduleDialogComponent {
  public form = this.formBuilder.group({
    id: 'ddf3c33a-7fa1-442d-9afc-7cac2edb8d3a',
    semiGroupsId: [['ddf3c33a-7fa1-442d-9afc-7cac2edb8d3a']],
    courseHourTypeId: ['', [Validators.required]],
    userId: ['', [Validators.required]],
    classroomId: ['', [Validators.required]],
    studyWeeks: [[0], [Validators.required]],
    dayOfWeek: [0, [Validators.required]],
    startTime: [0, [Validators.required]],
    endTime: [0, [Validators.required]],
  })

  courseHourTypes: ICourseHourType[] = [];
  users: IUser[] = [];
  studyPrograms: IStudyProgram[] = [];
  courses: ICourseResponse[] = [];
  classrooms: IClassroom[] = [];
  studyProgram = new FormControl;
  isSecretary:boolean=false;
  isProfessor:boolean=false;


  constructor(
    public dialogRef: MatDialogRef<ScheduleDialogComponent>,
    private formBuilder: FormBuilder,
    private snack: SnackBarService,
    private authService: AuthService,
    private scheduleService: ScheduleService,
    private studyProgramService: StudyProgramService,
    private courseService: CourseService,
    private courseHourTypeService: CourseHourTypeService,
    private classroomService: ClassroomService
  ) {
  }

  async ngOnInit(): Promise<void> {
    this.isSecretary=await this.authService.hasRole('Secretary');
    this.isProfessor=await this.authService.hasRole('Professor');
    if(this.isSecretary) {
      combineLatest([
        this.authService.getUsersByRole('Professor'),
        this.authService.getUsersByRole('LabAssistant'),
      ]).pipe(filter((r) => !!r[0] && !!r[1]))
        .subscribe(([professorUsers, labAssistantUsers]) => {
            this.users = professorUsers;
            this.users = this.users.concat(labAssistantUsers);
          }
        )
    }
    else {
      let professorSession= await this.authService.getSession();
      this.form.controls.userId.setValue(professorSession.userId);
      this.form.controls.userId.removeValidators([Validators.required]);
    }
    this.studyPrograms = await this.studyProgramService.getAll();
    this.classrooms = await this.classroomService.getAll();
  }

  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
      await this.scheduleService.create(this.form.value);
      this.snack.openSnackBar('Ora a fost adaugată cu succes');
      this.form.reset();
    } catch (e) {
      this.snack.showError(e);
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }


  public compareFn(optionOne: any, optionTwo: any): boolean {
    return optionOne.id === optionTwo.id;
  }

  public async setCoursesByStudyProgramId(id: string | null) {
    if (id){
      this.courses = await this.courseService.getCourseByStudyProgramId(id);
      if(this.isProfessor){
        let session=await this.authService.getSession()
        this.courses=this.courses.filter(c=>c.professorUser.id==session.userId);
      }
    }
  }

  public async setCourseHourTypesByCourseId(id: string | null) {
    if (id) this.courseHourTypes = await this.courseHourTypeService.getByCourseId(id);
  }

  public findClassroomDaysById(id: string | null) {
    let res = this.classrooms.find(c => c.id == id);
    if (res) {
      return res.daysOfWeek;
    }
    return [''];
  }

  public findStudyProgramActiveAndReturnAListOfStudyWeeks() {
    let studyWeeksArray: number[]=[];
    if (this.studyProgram.value && this.studyProgram.value.weeksInASemester) {
      for (let i = 1; i <= this.studyProgram.value.weeksInASemester; i++) {
        studyWeeksArray.push(i);
      }
      return studyWeeksArray
    }
    studyWeeksArray=[0];
    return studyWeeksArray;

  }

}
