import { MockApi } from './types';

export default (baseURL: string): Array<MockApi> => [
  {
    url: `${baseURL}/User/FlushKey`,
    method: 'get',
    response: () => ({
      succeed: true,
      msg: null,
      entity: {
        key: 'key',
        identity: 'identity',
      },
    }),
  },
  {
    url: `${baseURL}/User/Login`,
    method: 'post',
    response: () => ({
      succeed: true,
      msg: null,
      entity: {
      },
    }),
  },
];
