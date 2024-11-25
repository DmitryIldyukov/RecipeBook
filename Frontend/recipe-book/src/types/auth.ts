export type RegistrationInfo = {
  name: string;
  login: string;
  password: string;
};

export type LoginInfo = {
  login: string;
  password: string;
};

export type TokenInfo = {
  accessToken: string;
  refreshToken: string;
};
