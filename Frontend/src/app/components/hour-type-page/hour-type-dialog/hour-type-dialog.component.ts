import {Component, Inject} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {SnackBarService} from "../../../services/snack-bar.service";
import {HourTypeService} from "../../../services/hour-type.service";
import {IHourType} from "../../../models/hour-type";

@Component({
  selector: 'app-hour-type-dialog',
  templateUrl: './hour-type-dialog.component.html',
  styleUrls: ['./hour-type-dialog.component.scss']
})
export class HourTypeDialogComponent {
  public form = this.formBuilder.group({
    id: ['ddf3c33a-7fa1-442d-9afc-7cac2edb8d3a'],
    name: ['', [Validators.required]],
    semiGroupsPerHour: [1, [Validators.required]],
    needAllSemiGroups: [false],
  })

  constructor(
    public dialogRef: MatDialogRef<HourTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IHourType,
    private formBuilder: FormBuilder,
    private snack: SnackBarService,
    private hourTypeService: HourTypeService
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
        await this.hourTypeService.update(this.form.value);
        this.snack.openSnackBar('Tipul de oră fost actualizată cu succes');
      } else {
        await this.hourTypeService.create(this.form.value);
        this.snack.openSnackBar('Tipul de oră a fost adaugată cu succes');
        this.form.reset();
      }
    } catch (e) {
      this.snack.showError(e);
    }
  }

}
