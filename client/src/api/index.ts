import { AxiosPromise } from 'axios';
import { useUserApi } from './modules/user';
import fetcher from './fetcher';

type ApiModules = Record<
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  string, Record<string, (arg1?: any, arg2?: any) => AxiosPromise>
>;

export default (): ApiModules => ({
  user: useUserApi(fetcher),
});
