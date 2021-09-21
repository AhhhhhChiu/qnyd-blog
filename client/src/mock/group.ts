import { MockApi } from './types';

export default (baseURL: string): Array<MockApi> => [
  {
    url: `${baseURL}/Article/GetGroup`,
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
    url: `${baseURL}/Article/CreateGroup`,
    method: 'post',
    response: () => ({
      succeed: true,
      msg: null,
      entity: {},
    }),
  },
];
