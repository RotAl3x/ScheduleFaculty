import {Component} from '@angular/core';
import {ICourseResponse} from "../../models/course";
import {CourseService} from "../../services/course.service";
import {SnackBarService} from "../../services/snack-bar.service";
import {MatDialog} from "@angular/material/dialog";
import {CourseDialogComponent} from "./course-dialog/course-dialog.component";
import {IHourType} from "../../models/hour-type";

@Component({
  selector: 'app-course-page',
  templateUrl: './course-page.component.html',
  styleUrls: ['./course-page.component.scss']
})
export class CoursePageComponent {

  public courses: ICourseResponse[] | undefined;

  constructor(private courseService: CourseService,
              private snack: SnackBarService,
              public dialog: MatDialog) {
  }

  async ngOnInit() {
    this.courses = await this.courseService.getAll();
  }

  async delete(Course: ICourseResponse) {
    try {
      await this.courseService.delete(Course.id);
      this.snack.openSnackBar('S-a șters cu succes');
      this.courses?.splice(this.courses?.indexOf(Course), 1);
    } catch (e) {
      this.snack.showError(e);
    }
  }

  openDialog(Course: ICourseResponse | null): void {
    const dialogRef = this.dialog.open(CourseDialogComponent, {
      data: Course,
    });

    dialogRef.afterClosed().subscribe(async result => {
      this.courses = await this.courseService.getAll();
    });
  }

  hourTypesNameToString(hourTypes:IHourType[]):string{
    let res = '';
   hourTypes.forEach(x=>res+=x.name+' ');
   return res;
  }
}
