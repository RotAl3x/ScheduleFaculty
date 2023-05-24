import { Injectable } from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "./auth.service";
import {IClassroom} from "../models/classroom";
import {firstValueFrom} from "rxjs";
import {IAssignedCourseUser, IAssignedCourseUserResponse} from "../models/assigned-course-user";

@Injectable({
  providedIn: 'root'
})
export class AssignedCourseUserService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private authService:AuthService){
  }

  public async getAll(): Promise<IAssignedCourseUserResponse[]> {
    const url = this._baseUrl + 'api/assignedCourseUser/getAll'
    return await firstValueFrom(this.http.get<IAssignedCourseUserResponse[]>(url));
  }

  public async getByCourseId(data:string): Promise<IAssignedCourseUserResponse[]> {
    const url = this._baseUrl + 'api/assignedCourseUser/getByCourseId'+data;
    return await firstValueFrom(this.http.get<IAssignedCourseUserResponse[]>(url));
  }

  public async getByUserId(data:string): Promise<IAssignedCourseUserResponse[]> {
    const url = this._baseUrl + 'api/assignedCourseUser/getByUserId'+data;
    return await firstValueFrom(this.http.get<IAssignedCourseUserResponse[]>(url));
  }

  public async create(data:Partial<IAssignedCourseUser>): Promise<IAssignedCourseUserResponse> {
    const url = this._baseUrl + 'api/assignedCourseUser/create';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.post<IAssignedCourseUserResponse>(url,data,options));
  }

  public async delete(data:string): Promise<any> {
    const url = this._baseUrl + 'api/assignedCourseUser/delete'+data;
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.delete(url,options));
  }

}
