import Mock from 'mockjs';
import { MockApi } from './types';
import { baseURL } from '@/api/fetcher';
import useUserMock from './user';

const mockApis: Array<MockApi> = [
  ...useUserMock(baseURL),
];

mockApis.forEach((api) => {
  Mock.mock(
    api.url,
    api.method,
    api.response,
  );
});
