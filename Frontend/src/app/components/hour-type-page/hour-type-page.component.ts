import { Component } from '@angular/core';
import {IStudyProgram} from "../../models/study-program";
import {StudyProgramService} from "../../services/study-program.service";
import {SnackBarService} from "../../services/snack-bar.service";
import {MatDialog} from "@angular/material/dialog";
import {StudyProgramDialogComponent} from "../study-program-page/study-program-dialog/study-program-dialog.component";
import {IHourType} from "../../models/hour-type";
import {HourTypeService} from "../../services/hour-type.service";
import {HourTypeDialogComponent} from "./hour-type-dialog/hour-type-dialog.component";

@Component({
  selector: 'app-hour-type-page',
  templateUrl: './hour-type-page.component.html',
  styleUrls: ['./hour-type-page.component.scss']
})
export class HourTypePageComponent {


  public hourTypes: IHourType[] | undefined;

  constructor(private hourTypeService: HourTypeService,
              private snack: SnackBarService,
              public dialog: MatDialog) {
  }

  async ngOnInit() {
    this.hourTypes = await this.hourTypeService.getAll();
  }

  async delete(hourType: IHourType) {
    try {
      await this.hourTypeService.delete(hourType.id);
      this.snack.openSnackBar('S-a șters cu succes');
      this.hourTypes?.splice(this.hourTypes?.indexOf(hourType), 1);
    } catch (e) {
      this.snack.showError(e);
    }
  }

  openDialog(hourType: IHourType | null): void {
    const dialogRef = this.dialog.open(HourTypeDialogComponent, {
      data: hourType,
    });

    dialogRef.afterClosed().subscribe(async result => {
      this.hourTypes = await this.hourTypeService.getAll();
    });
  }
}
