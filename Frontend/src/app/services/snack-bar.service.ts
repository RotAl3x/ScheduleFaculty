import {Injectable} from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {

  constructor(private _snackBar: MatSnackBar) {
  }

  openSnackBar(message: string) {
    this._snackBar.open(message, 'Close', {duration: 3000});
  }

  public showError(response: any): void {
    try {
      this.openSnackBar(response.error);
    } catch (e) {
      this.openSnackBar('Oops! Ceva nu a mers bine!');
    }
  }
}
