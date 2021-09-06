import { useUserApi } from './modules/user';
import fetcher from './fetcher';

export default () => ({
  user: useUserApi(fetcher),
});
