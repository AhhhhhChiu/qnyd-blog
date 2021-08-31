import { AxiosInstance, AxiosPromise } from 'axios';

type LandingData = {
  userName: string,
  passwordHash: string,
  connectId: string,
};

type UserApi = {
  getFlushKey: () => AxiosPromise,
  register: (data: LandingData) => AxiosPromise,
  login: (data: LandingData) => AxiosPromise,
};

export default (fetcher: AxiosInstance): UserApi => {
  /** 获取公钥 */
  const getFlushKey = (): AxiosPromise => fetcher({
    method: 'get',
    url: '/User/FlushKey',
  });

  /** 注册 */
  const register = (data: LandingData): AxiosPromise => fetcher({
    data,
    method: 'post',
    url: '/User​/Registe',
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
