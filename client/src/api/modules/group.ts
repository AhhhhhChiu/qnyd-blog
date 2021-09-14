import { AxiosInstance, AxiosPromise } from 'axios';

export type GetGroupParams = {
  skip: number,
  take: number,
};

export type CreateGroupParams = {
  name: string,
  tags: Array<string>,
};

export type Group = {
  createTime: string,
  id: string,
  name: string,
  tags: Array<string>,
};

export type GetGroupResponse = {
  data: {
    entity: Array<Group>,
    msg: string | null,
    skip: string |null,
    succeed: boolean,
    take: string |null,
    total: number,
  }
};

export type GroupApi = {
  getGroup: (params: GetGroupParams) => AxiosPromise,
  createGroup: (params: CreateGroupParams) => AxiosPromise,
};

export const useGroupApi = (fetcher: AxiosInstance): GroupApi => {
  const getGroup = (params?: GetGroupParams) => fetcher({
    method: 'get',
    url: '/Article/GetGroup',
    params,
  });
  const createGroup = (data: CreateGroupParams) => fetcher({
    method: 'post',
    url: '/Article/CreateGroup',
    data,
  });
  return {
    getGroup,
    createGroup,
  };
};
