import {Component} from '@angular/core';
import {IStudyProgram} from "../../models/study-program";
import {SnackBarService} from "../../services/snack-bar.service";
import {MatDialog} from "@angular/material/dialog";
import {StudyProgramService} from "../../services/study-program.service";
import {StudyProgramDialogComponent} from "./study-program-dialog/study-program-dialog.component";

@Component({
  selector: 'app-study-program-page',
  templateUrl: './study-program-page.component.html',
  styleUrls: ['./study-program-page.component.scss']
})
export class StudyProgramPageComponent {

  public studyPrograms: IStudyProgram[] | undefined;

  constructor(private studyProgramService: StudyProgramService,
              private snack: SnackBarService,
              public dialog: MatDialog) {
  }

  async ngOnInit() {
    this.studyPrograms = await this.studyProgramService.getAll();
  }

  async delete(studyProgram: IStudyProgram) {
    try {
      await this.studyProgramService.delete(studyProgram.id);
      this.snack.openSnackBar('S-a șters cu succes');
      this.studyPrograms?.splice(this.studyPrograms?.indexOf(studyProgram), 1);
    } catch (e) {
      this.snack.showError(e);
    }
  }

  openDialog(studyProgram: IStudyProgram | null): void {
    const dialogRef = this.dialog.open(StudyProgramDialogComponent, {
      data: studyProgram,
    });

    dialogRef.afterClosed().subscribe(async result => {
      this.studyPrograms = await this.studyProgramService.getAll();
    });
  }

}
