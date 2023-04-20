export interface ILogin {
  email: string|null;
  password: string|null;
}

export interface IAuthSession{
  userId: string;
  username: string;
  token: string;
  tokenType: string;
  role: string;
}
export interface IRegister {
  email: string,
  password: string,
  confirmPassword: string,
  firstName: string,
  lastName: string
}

export interface IChangePassword {
  currentPassword: string,
  newPassword: string,
  repeatPassword: string,
}

