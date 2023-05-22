import { Injectable } from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "./auth.service";
import {IHourStudyOfAYear} from "../models/hour-study-of-ayear";
import {firstValueFrom} from "rxjs";
import {ICourseHourType} from "../models/course-hour-type";

@Injectable({
  providedIn: 'root'
})
export class CourseHourTypeService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  public async getByCourseId(data: string): Promise<ICourseHourType[]> {
    const url = this._baseUrl + 'api/courseHourType/byCourseId/' + data;
    return await firstValueFrom(this.http.get<ICourseHourType[]>(url));
  }
}
