import {Component, OnInit} from '@angular/core';
import {ClassroomService} from "../../services/classroom.service";
import {IClassroom} from "../../models/classroom";
import {SnackBarService} from "../../services/snack-bar.service";
import {MatDialog} from "@angular/material/dialog";
import {ClassroomDialogComponent} from "./classroom-dialog/classroom-dialog.component";

@Component({
  selector: 'app-classroom-page',
  templateUrl: './classroom-page.component.html',
  styleUrls: ['./classroom-page.component.scss']
})
export class ClassroomPageComponent implements OnInit {

  public classrooms: IClassroom[] | undefined;

  constructor(private classroomService: ClassroomService,
              private snack: SnackBarService,
              public dialog: MatDialog) {
  }

  async ngOnInit() {
    this.classrooms = await this.classroomService.getAll();
  }

  async delete(classroom: IClassroom) {
    try {
      await this.classroomService.delete(classroom.id);
      this.snack.openSnackBar('S-a șters cu succes');
      this.classrooms?.splice(this.classrooms?.indexOf(classroom), 1);
    } catch (e) {
      this.snack.showError(e);
    }
  }

  openDialog(classroom: IClassroom | null): void {
    const dialogRef = this.dialog.open(ClassroomDialogComponent, {
      data: classroom,
    });

    dialogRef.afterClosed().subscribe(async result => {
      this.classrooms = await this.classroomService.getAll();
    });
  }

  weekDaysArrayToString(daysOfWeek: string[] | null) {
    let res: string = ''
    if (daysOfWeek)
      daysOfWeek.forEach(d => res += (d + ' '));
    return res;
  }
}
