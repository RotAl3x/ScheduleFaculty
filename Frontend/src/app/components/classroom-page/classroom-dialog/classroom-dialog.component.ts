import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {IClassroom} from "../../../models/classroom";
import {FormBuilder, UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import {SnackBarService} from "../../../services/snack-bar.service";
import {ClassroomService} from "../../../services/classroom.service";
import {WeekDay} from "@angular/common";
import {ILogin} from "../../../models/login";
import {delay} from "rxjs";

@Component({
  selector: 'app-classroom-dialog',
  templateUrl: './classroom-dialog.component.html',
  styleUrls: ['./classroom-dialog.component.scss']
})
export class ClassroomDialogComponent implements OnInit {
  public form = this.formBuilder.group({
    name: ['', [Validators.required]],
    daysOfWeek: [[''], [Validators.required]]
  })

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
      this.form.controls.name.patchValue(this.data.name);
      this.form.controls.daysOfWeek.patchValue(this.data.daysOfWeek)
    }
  }

  async submit(e: Event) {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      this.snack.openSnackBar('Verifică formularul');
      return;
    }
    try {
      let request: IClassroom = {
        name: this.form.controls.name.value,
        daysOfWeek: this.form.controls.daysOfWeek.value
      };
      if (this.data) {
        request.id = this.data.id;
        await this.classroomService.update(request);
        this.snack.openSnackBar('Clasa a fost actualizată cu succes');
      } else {
        await this.classroomService.create(request);
        this.snack.openSnackBar('Clasa a fost adaugată cu succes');
      }
    } catch (e) {
      this.snack.showError(e);
    }
  }

  keys(): Array<string> {
    var keys = Object.keys(this.WeekDay);
    return keys.slice(keys.length / 2);
  }

  public readonly WeekDay = WeekDay;
}
