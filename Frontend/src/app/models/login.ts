export interface ILogin {
  email: string | null;
  password: string | null;
}

export interface IAuthSession {
  userId: string;
  username: string;
  token: string;
  tokenType: string;
  role: string;
}

export interface IRegister {
  email: string|null,
  firstName: string|null,
  lastName: string|null,
  role: string|null,
}

export interface IChangePassword {
  currentPassword: string | null,
  newPassword: string | null,
  repeatPassword: string | null,
}

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
}

export interface IRole{
  "id": string,
  "name": string,
  "normalizedName": string,
  "concurrencyStamp": null,
}

