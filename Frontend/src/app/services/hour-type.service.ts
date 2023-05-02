import {Injectable} from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AuthService} from "./auth.service";
import {firstValueFrom} from "rxjs";
import {IHourType} from "../models/hour-type";

@Injectable({
  providedIn: 'root'
})
export class HourTypeService {
  private readonly _baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,
              private router: Router,
              private authService: AuthService) {
  }

  public async getAll(): Promise<IHourType[]> {
    const url = this._baseUrl + 'api/hourType/allHourType'
    return await firstValueFrom(this.http.get<IHourType[]>(url));
  }

  public async create(data: Partial<IHourType>): Promise<IHourType> {
    const url = this._baseUrl + 'api/hourType';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.post<IHourType>(url, data, options));
  }

  public async update(data: Partial<IHourType>): Promise<IHourType> {
    const url = this._baseUrl + 'api/hourType/edit';
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.patch<IHourType>(url, data, options));
  }

  public async delete(data: string | null): Promise<any> {
    const url = this._baseUrl + 'api/hourType/delete/' + data;
    const options = await this.authService.getOptions(true);
    return await firstValueFrom(this.http.delete<string>(url, options));
  }

  public async getById(data: string): Promise<IHourType> {
    const url = this._baseUrl + 'api/hourType/' + data;
    return await firstValueFrom(this.http.get<IHourType>(url));
  }
}
