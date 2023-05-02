import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {IClassroom} from "../../../models/classroom";
import {FormBuilder, Validators} from "@angular/forms";
import {SnackBarService} from "../../../services/snack-bar.service";
import {ClassroomService} from "../../../services/classroom.service";
import {WeekDay} from "@angular/common";

@Component({
  selector: 'app-classroom-dialog',
  templateUrl: './classroom-dialog.component.html',
  styleUrls: ['./classroom-dialog.component.scss']
})
export class ClassroomDialogComponent implements OnInit {
  public form = this.formBuilder.group({
    id: ['ddf3c33a-7fa1-442d-9afc-7cac2edb8d3a'],
    name: ['', [Validators.required]],
    daysOfWeek: [[''], [Validators.required]]
  })
  public readonly WeekDay = WeekDay;

  constructor(
    public dialogRef: MatDialogRef<ClassroomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IClassroom,
    private formBuilder: FormBuilder,
    private snack: SnackBarService,
    private classroomService: ClassroomService
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
        await this.classroomService.update(this.form.value);
        this.snack.openSnackBar('Clasa a fost actualizată cu succes');
      } else {
        await this.classroomService.create(this.form.value);
        this.snack.openSnackBar('Clasa a fost adaugată cu succes');
        this.form.reset();
      }
    } catch (e) {
      this.snack.showError(e);
    }
  }

  keys(): Array<string> {
    var keys = Object.keys(this.WeekDay);
    return keys.slice(keys.length / 2);
  }
}
