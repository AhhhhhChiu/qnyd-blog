import axios, {
  AxiosResponse, AxiosInstance, AxiosPromise, AxiosRequestConfig,
} from 'axios';
import { ElMessage } from 'element-plus';

const baseURL = '/api/v1';

const instance: AxiosInstance = axios.create({
  baseURL,
  timeout: 60000,
  withCredentials: true,
});

const handleError = (status: number, message: string) => {
  switch (status) {
    case 404:
      // to not found page
      ElMessage.error(`Page not fount: ${message}`);
      break;
    default:
      ElMessage.error(`Unknown error: ${message}`);
  }
};

instance.interceptors.response.use(
  (response: AxiosResponse): AxiosPromise => Promise.resolve(response),
  (error): AxiosPromise => {
    const { response } = error;
    if (response) {
      handleError(response.status, response.data.message);
    } else {
      ElMessage.error('网络开小差~');
    }
    return Promise.reject(error);
  },
);

instance.interceptors.request.use(
  (config): Promise<AxiosRequestConfig> => {
    if (config.data) {
      let parsedUrl: string | undefined = config.url;
      if (config.data.rParams) {
        const { rParams } = config.data;
        Object.keys(rParams).forEach((key) => {
          parsedUrl = (parsedUrl as string).replace(`:${key}`, rParams[key]);
        });
        parsedUrl = (parsedUrl as string).replace(/\/:\w*$/, '');
      }
      return Promise.resolve({
        ...config,
        url: parsedUrl,
        data: config.data.data,
      });
    }
    return Promise.resolve(config);
  },
  (error): AxiosPromise => Promise.reject(error),
);

export default instance;
