import {Component} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {IStatus} from "../../models/status";
import {StatusService} from "../../services/status.service";
import {StatusDialogComponent} from "./status-dialog/status-dialog.component";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-status-page',
  templateUrl: './status-page.component.html',
  styleUrls: ['./status-page.component.scss']
})
export class StatusPageComponent {

  public statuses: IStatus[] | undefined;
  public isSecretary:boolean=false;

  constructor(private statusService: StatusService,
              private authService:AuthService,
              public dialog: MatDialog) {
  }

  async ngOnInit() {
    this.statuses = await this.statusService.getAll();
    this.isSecretary=await this.authService.hasRole('Secretary');
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
