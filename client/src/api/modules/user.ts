import { AxiosInstance, AxiosPromise, SuccessData } from 'axios';

export type LandingData = {
  userName: string,
  passwordHash: string,
  connectId: string,
};

export type GetFlushKeyResponse = {
  succeed: boolean,
  msg: string | null,
  entity: {
    key: string,
    identity: string,
  },
};

export type UserApi = {
  getFlushKey: () => AxiosPromise,
  register: (data: LandingData) => AxiosPromise,
  login: (data: LandingData) => AxiosPromise,
};

export const useUserApi = (fetcher: AxiosInstance): UserApi => {
  /** 获取公钥 */
  const getFlushKey = (): AxiosPromise => fetcher({
    method: 'get',
    url: '/User/FlushKey',
  });

  /** 注册 */
  const register = (data: LandingData): AxiosPromise => fetcher({
    data,
    method: 'post',
    url: '/User/Registe',
  });

  /** 登录 */
  const login = (data: LandingData): AxiosPromise => fetcher({
    data,
    method: 'post',
    url: '/User​/Login',
  });

  return {
    getFlushKey,
    register,
    login,
  };
};
