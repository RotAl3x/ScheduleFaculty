import {Injectable} from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "./auth.service";
import {ICourse, ICourseResponse} from "../models/course";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  public async getAll(): Promise<ICourseResponse[]> {
    const url = this._baseUrl + 'api/course/getAllCourses'
    return await firstValueFrom(this.http.get<ICourseResponse[]>(url));
  }

  public async create(data: Partial<ICourse>): Promise<ICourse> {
    const url = this._baseUrl + 'api/course';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.post<ICourse>(url, data, options));
  }

  public async update(data: Partial<ICourse>): Promise<ICourse> {
    const url = this._baseUrl + 'api/course/edit';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.patch<ICourse>(url, data, options));
  }

  public async delete(data: string | undefined): Promise<any> {
    const url = this._baseUrl + 'api/course/delete/' + data;
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.delete<string>(url, options));
  }

  public async getById(data: string): Promise<ICourseResponse> {
    const url = this._baseUrl + 'api/course/' + data;
    return await firstValueFrom(this.http.get<ICourseResponse>(url));
  }

  public async getByProfessorId(data: string): Promise<ICourseResponse[]> {
    const url = this._baseUrl + 'api/course/getByProfessorId/' + data;
    return await firstValueFrom(this.http.get<ICourseResponse[]>(url));
  }

  public async getCourseByStudyProgramId(data: string): Promise<ICourseResponse[]> {
    const url = this._baseUrl + 'api/course/getCourseByStudyProgramId/' + data;
    return await firstValueFrom(this.http.get<ICourseResponse[]>(url));
  }
}
