import fetcher from './fetcher';
import useUserApi from './modules/user';

export default () => ({
  user: useUserApi(fetcher),
});
