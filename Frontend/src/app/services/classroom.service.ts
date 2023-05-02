import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {environment} from "../../environment/environment";
import {firstValueFrom} from "rxjs";
import {IClassroom} from "../models/classroom";
import {AuthService} from "./auth.service";

@Injectable({
  providedIn: 'root'
})
export class ClassroomService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  public async getAll(): Promise<IClassroom[]> {
    const url = this._baseUrl + 'api/classroom/getAll'
    return await firstValueFrom(this.http.get<IClassroom[]>(url));
  }

  public async create(data: Partial<IClassroom>): Promise<IClassroom> {
    const url = this._baseUrl + 'api/classroom';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.post<IClassroom>(url, data, options));
  }

  public async update(data: Partial<IClassroom>): Promise<IClassroom> {
    const url = this._baseUrl + 'api/classroom/edit';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.patch<IClassroom>(url, data, options));
  }

  public async delete(data: string | null): Promise<any> {
    const url = this._baseUrl + 'api/classroom/delete/' + data;
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.delete<string>(url, options));
  }

  public async getById(data: string): Promise<IClassroom> {
    const url = this._baseUrl + 'api/classroom/' + data;
    return await firstValueFrom(this.http.get<IClassroom>(url));
  }

}
