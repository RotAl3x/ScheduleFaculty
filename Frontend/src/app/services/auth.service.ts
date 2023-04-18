import {EventEmitter, Injectable} from '@angular/core';
import {environment} from "../../environment/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {LocalStorage} from "@ngx-pwa/local-storage";
import {IAuthSession, IChangePassword, ILogin, IRegister} from "../models/login";
import {firstValueFrom, map, tap} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _token?: string;
  private _session?: IAuthSession;
  private static readonly tokenStorageKey: string = 'token';
  private static readonly sessionStorageKey: string = 'session';
  private _authState: EventEmitter<boolean> = new EventEmitter();
  private readonly _baseUrl= environment.apiUrl;


  constructor(private http: HttpClient,
              private storage: LocalStorage,
              private router: Router) {
  }

  public async login(requestModel: ILogin): Promise<any> {
    const url = this._baseUrl + 'api/auth/login';
    return firstValueFrom(this.http.post<IAuthSession>(url, requestModel)
      .pipe(tap(async res => {
        await this.saveSession(res);
      }))
      .pipe(map(() => {
        return true;
      })));
  }

  public async changePassword(data: IChangePassword):Promise<any>{
    const url = this._baseUrl+'api/auth/changePassword';
    return firstValueFrom(this.http.post(url, data, {responseType: 'text'}));
  }

  public registerSecretary(data: IRegister): Promise<any> {
    const url = this._baseUrl+'api/auth/registerSecretary';
    return firstValueFrom(this.http.post(url, data, {responseType: 'text'}));
  }

  public registerProfessor(data: IRegister): Promise<any> {
    const url = this._baseUrl+'api/auth/registerProfessor';
    return firstValueFrom(this.http.post(url, data, {responseType: 'text'}));
  }

  public registerLabAssistant(data: IRegister): Promise<any> {
    const url = this._baseUrl+'api/auth/LabAssistant';
    return firstValueFrom(this.http.post(url, data, {responseType: 'text'}));
  }

  public async saveSession(authSession?: IAuthSession): Promise<void> {
    if (authSession) {
      await firstValueFrom(this.storage.setItem(AuthService.tokenStorageKey, authSession.token));
      await firstValueFrom(this.storage.setItem(AuthService.sessionStorageKey, authSession));
    } else {
      await firstValueFrom(this.storage.removeItem(AuthService.tokenStorageKey));
      await firstValueFrom(this.storage.removeItem(AuthService.sessionStorageKey));
    }
    await this.loadSession();
  }

  private async loadSession(): Promise<void> {
    const initialStatus = !!this._token;
    const oldToken = this._token;
    this._token = <string>await firstValueFrom(this.storage.getItem(AuthService.tokenStorageKey));
    if (this._token) {
      this._session = <IAuthSession>await firstValueFrom(this.storage.getItem(AuthService.sessionStorageKey));
    } else {
      this._session = undefined;
    }
    const differentStatus = initialStatus !== !!this._token || oldToken !== this._token;
    if (differentStatus) {
      this._authState.emit(!!this._token);
    }
  }

  public async getOptions(needsAuth?: boolean): Promise<{ headers?: HttpHeaders }> {
    return {headers: await this.getHeaders(needsAuth)};
  }

  public async hasRole(role: string): Promise<boolean> {
    const session = await this.getSession();
    if (!session || !session.role) {
      return false;
    }

    return session.role.indexOf(role) !== -1;
  }

  public async getHeaders(needsAuth?: boolean): Promise<HttpHeaders> {
    if (!needsAuth) {
      return new HttpHeaders();
    }
    const session = await this.getSession();

    if (!session) {
      return new HttpHeaders();
    }

    return new HttpHeaders().append('Authorization', `${session.tokenType} ${session.token}`);
  }

  public async getSession(): Promise<IAuthSession> {
    if (!this._session) {
      this._session = <IAuthSession>await firstValueFrom(this.storage.getItem(AuthService.sessionStorageKey));
    }
    return this._session;
  }


  public async logout(): Promise<void> {
    await this.saveSession();
    await this.router.navigate(['/']);
  }
}
