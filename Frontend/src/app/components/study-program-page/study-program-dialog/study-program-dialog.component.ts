import {Component, Inject} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {SnackBarService} from "../../../services/snack-bar.service";
import {IStudyProgram} from "../../../models/study-program";
import {StudyProgramService} from "../../../services/study-program.service";

@Component({
  selector: 'app-study-program-dialog',
  templateUrl: './study-program-dialog.component.html',
  styleUrls: ['./study-program-dialog.component.scss']
})
export class StudyProgramDialogComponent {
  public form = this.formBuilder.group({
    id: ['ddf3c33a-7fa1-442d-9afc-7cac2edb8d3a'],
    name: ['', [Validators.required]],
    year: [1, [Validators.required]],
    weeksInASemester: [0, [Validators.required]],
    numberOfSemiGroups: [0, [Validators.required]],
    howManySemiGroupsAreInAGroup: [0, [Validators.required]],
  })

  constructor(
    public dialogRef: MatDialogRef<StudyProgramDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IStudyProgram,
    private formBuilder: FormBuilder,
    private snack: SnackBarService,
    private studyProgramService: StudyProgramService
  ) {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit(): void {
    if (this.data) {
      this.form.patchValue(this.data);
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
        await this.studyProgramService.update(this.form.value);
        this.snack.openSnackBar('Programul de studiu a fost actualizată cu succes');
      } else {
        await this.studyProgramService.create(this.form.value);
        this.snack.openSnackBar('Programul de studiu a fost adaugată cu succes');
        this.form.reset();
      }
    } catch (e) {
      this.snack.showError(e);
    }
  }

}
