import {Injectable} from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "./auth.service";
import {IStudyProgram} from "../models/study-program";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StudyProgramService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  public async getAll(): Promise<IStudyProgram[]> {
    const url = this._baseUrl + 'api/studyProgram/getAll'
    return await firstValueFrom(this.http.get<IStudyProgram[]>(url));
  }

  public async create(data: Partial<IStudyProgram>): Promise<IStudyProgram> {
    const url = this._baseUrl + 'api/studyProgram';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.post<IStudyProgram>(url, data, options));
  }

  public async update(data: Partial<IStudyProgram>): Promise<IStudyProgram> {
    const url = this._baseUrl + 'api/studyProgram/edit';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.patch<IStudyProgram>(url, data, options));
  }

  public async delete(data: string | null): Promise<any> {
    const url = this._baseUrl + 'api/studyProgram/delete/' + data;
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.delete<string>(url, options));
  }

  public async getById(data: string): Promise<IStudyProgram> {
    const url = this._baseUrl + 'api/studyProgram/' + data;
    return await firstValueFrom(this.http.get<IStudyProgram>(url));
  }

}
