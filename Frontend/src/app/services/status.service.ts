import {Injectable} from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "./auth.service";
import {IStatus} from "../models/status";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  public async getAll(): Promise<IStatus[]> {
    const url = this._baseUrl + 'api/status/allStatuses'
    return await firstValueFrom(this.http.get<IStatus[]>(url));
  }

  public async update(data: Partial<IStatus>): Promise<IStatus> {
    const url = this._baseUrl + 'api/status/edit';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.patch<IStatus>(url, data, options));
  }

  public async getById(data: string): Promise<IStatus> {
    const url = this._baseUrl + 'api/status/' + data;
    return await firstValueFrom(this.http.get<IStatus>(url));
  }

  public async getActiveStatus(): Promise<IStatus> {
    const url = this._baseUrl + 'api/status/activeStatus';
    return await firstValueFrom(this.http.get<IStatus>(url));
  }
}
