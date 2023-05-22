import {Component, OnInit} from '@angular/core';
import {FormControl} from "@angular/forms";
import {ClassroomService} from "../../services/classroom.service";
import {StudyProgramService} from "../../services/study-program.service";
import {ScheduleService} from "../../services/schedule.service";
import {IClassroom} from "../../models/classroom";
import {IStudyProgram} from "../../models/study-program";
import {IHourStudyOfAYear, IHourStudyOfAYearResponse} from "../../models/hour-study-of-ayear";
import {ICourseResponse} from "../../models/course";
import {CourseDialogComponent} from "../course-page/course-dialog/course-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {ScheduleDialogComponent} from "./schedule-dialog/schedule-dialog.component";
import {WeekDay} from "@angular/common";
import {SnackBarService} from "../../services/snack-bar.service";
import {IStudyYearGroup} from "../../models/study-year-group";

@Component({
  selector: 'app-schedule-page',
  templateUrl: './schedule-page.component.html',
  styleUrls: ['./schedule-page.component.scss']
})
export class SchedulePageComponent implements OnInit {
  public wordsToSearch = ['Program de studiu', 'Sală'];
  public wordSearch = new FormControl;
  public classrooms: IClassroom[] = [];
  public studyPrograms: IStudyProgram[] = [];
  public schedules: IHourStudyOfAYearResponse[] = [];

  constructor(private classroomService: ClassroomService,
              private studyProgramService: StudyProgramService,
              private scheduleService: ScheduleService,
              private snack:SnackBarService,
              public dialog: MatDialog) {
  }

  async ngOnInit() {
    this.classrooms = await this.classroomService.getAll();
    this.studyPrograms = await this.studyProgramService.getAll();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ScheduleDialogComponent);
  }

  async getScheduleByClassroomId(id: string | null) {
    if (id) {
      try {
        this.schedules = await this.scheduleService.getByClassroomId(id);
      }
      catch (e){
        this.snack.showError(e);
        this.schedules=[]
      }
    }
  }

  async getScheduleByStudyProgramId(id: string | null) {
    if (id) {
      try {
        this.schedules = await this.scheduleService.getByStudyProgramId(id);
      }
      catch (e){
        this.snack.showError(e);
        this.schedules=[]
      }
    }
  }

  semiGroupsToString(semigroups:IStudyYearGroup[]){
    let res:string='';
    semigroups.forEach(s=>res = res +' ' + s.name);
    return res;
  }

  protected readonly WeekDay = WeekDay;
}
