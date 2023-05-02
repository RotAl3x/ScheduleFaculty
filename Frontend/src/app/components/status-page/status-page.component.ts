import {Component} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {IStatus} from "../../models/status";
import {StatusService} from "../../services/status.service";
import {StatusDialogComponent} from "./status-dialog/status-dialog.component";

@Component({
  selector: 'app-status-page',
  templateUrl: './status-page.component.html',
  styleUrls: ['./status-page.component.scss']
})
export class StatusPageComponent {

  public statuses: IStatus[] | undefined;

  constructor(private statusService: StatusService,
              public dialog: MatDialog) {
  }

  async ngOnInit() {
    this.statuses = await this.statusService.getAll();
  }

  openDialog(status: IStatus | null): void {
    const dialogRef = this.dialog.open(StatusDialogComponent, {
      data: status,
    });

    dialogRef.afterClosed().subscribe(async result => {
      this.statuses = await this.statusService.getAll();
    });
  }


}
