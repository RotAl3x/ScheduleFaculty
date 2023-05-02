import {Component, Inject} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {SnackBarService} from "../../../services/snack-bar.service";
import {IStatus} from "../../../models/status";
import {StatusService} from "../../../services/status.service";

@Component({
  selector: 'app-status-dialog',
  templateUrl: './status-dialog.component.html',
  styleUrls: ['./status-dialog.component.scss']
})
export class StatusDialogComponent {
  public form = this.formBuilder.group({
    id: ['ddf3c33a-7fa1-442d-9afc-7cac2edb8d3a'],
    name: ['', [Validators.required]],
    semester: [0],
    isActive: [false],
  })

  constructor(
    public dialogRef: MatDialogRef<StatusDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IStatus,
    private formBuilder: FormBuilder,
    private snack: SnackBarService,
    private statusService: StatusService
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
      await this.statusService.update(this.form.value);
      this.snack.openSnackBar('Statusul a fost actualizată cu succes');
    } catch (e) {
      this.snack.showError(e);
    }
  }
}
