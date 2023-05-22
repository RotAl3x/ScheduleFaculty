import { Injectable } from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "./auth.service";
import {IClassroom} from "../models/classroom";
import {firstValueFrom} from "rxjs";
import {IHourStudyOfAYear, IHourStudyOfAYearResponse} from "../models/hour-study-of-ayear";
import {ICourse} from "../models/course";

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  public async getByClassroomId(data: string): Promise<IHourStudyOfAYearResponse[]> {
    const url = this._baseUrl + 'api/hourStudyOfAYear/classroomId/' + data;
    return await firstValueFrom(this.http.get<IHourStudyOfAYearResponse[]>(url));
  }

  public async getByStudyProgramId(data: string): Promise<IHourStudyOfAYearResponse[]> {
    const url = this._baseUrl + 'api/hourStudyOfAYear/studyProgramId/' + data;
    return await firstValueFrom(this.http.get<IHourStudyOfAYearResponse[]>(url));
  }

  public async create(data:Partial<IHourStudyOfAYear>): Promise<IHourStudyOfAYear>{
    const url = this._baseUrl + 'api/hourStudyOfAYear';
    const options = await this.authService.getOptions(true);
    console.log(options);
    return await firstValueFrom(this.http.post<IHourStudyOfAYear>(url, data, options));
  }
}
